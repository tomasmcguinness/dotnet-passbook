#dotnet-passbook
A .Net Library for generating Passbook packages for iOS 6

##Why
The goal of this project is to build a library that can generate, sign and zip Passbook files for use with iOS 6's Passbook. 

## Requirements

The solution requires .Net 4.5 and Visual Studio 2012.

##Certificates

Before you run the PassGenerator, you need to ensure you have all the necessary certificates installed. There are two required. 

Firstly, you need to install the Apple WWDR (WorldWide Developer Relations) certificate. You can download that from here [http://www.apple.com/certificateauthority/](http://www.apple.com/certificateauthority/) . You must install it in the Local Machine's Intermediate Certificate store. I've hardcoded that location 

Secondly, you need to installed your Passbook certificate, which you get from the Developer Portal. You must have a full iPhone developer account. There are [instructions on my blog](http://www.tomasmcguinness.com/2012/06/28/generating-an-apple-ios-certificate-using-windows/) for generating a certificate if you're using a Windows Machine.

You can place this certificate in any of the stores, but it must be placed into the "personal" folder.  When constructing the request for the pass, you specify the location and thumbprint for the certificate. If running this code in IIS for example, installing the certificate in the Local Machine area might make access easier. Alternatively, you could place the certificate into the AppPool's user's certification repository. When you install the certificate, be sure to note the certificate's Thumbprint. 

##Technical Stuff

To generate a pass, start by declaring a PassGenerator.

    PassGenerator generator = new PassGenerator();

Next, create a PassGeneratorRequest. This is a raw request that gives you the full power to add all the fields necessary for the pass you wish to create. Each pass is broken down into several sections. Each section is rendered in a different way, based on the style of the pass you are trying to produce. For more information on this, please consult Apple's Passbook Programming Guide. The example below here will show how to create a very basic boarding pass.

Since each pass has a set of mandatory data, fill that in first. 

    PassGeneratorRequest request = new PassGeneratorRequest();
    request.Identifier = "pass.tomsamcguinness.events";   
    request.TeamIdentifier = "RW121242";
    request.SerialNumber = "121212";
    request.Description = "My first pass";
    request.OrganizationName = "Tomas McGuinness";
    request.LogoText = "My Pass";
    request.BackgroundColor = "#FFFFFF";
    request.ForegroundColor = "#000000";

Choose the location of your Passbook certificate. This is used to sign the manifest.

 	request.CertThumbprint = "22C5FADDFBF935E26E2DDB8656010C7D4103E02E";
    request.CertLocation = System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser;

Next, define the images you with to use. You must always include both standard and retina sized images. Images are supplied as byte[].

    // override icon and icon retina
    request.Images.Add(PassbookImage.Icon, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon.png")));
    request.Images.Add(PassbookImage.IconRetina, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon@2x.png")));

You can now provide more pass specific information. The Style must be set and then all information is then added to fields to the required sections. For a baording pass, the fields are add to three sections;  primary, secondary and auxiliary.

	request.Style = PassStyle.BoardingPass;

    request.AddPrimaryField(new StandardField("origin", "San Francisco", "SFO"));
    request.AddPrimaryField(new StandardField("destination", "London", "LDN"));

    request.AddSecondaryField(new StandardField("boarding-gate", "Gate", "A55"));

    request.AddAuxiliaryField(new StandardField("seat", "Seat", "G5" ));
    request.AddAuxiliaryField(new StandardField("passenger-name", "Passenger", "Thomas Anderson"));

 	request.TransitType = TransitType.PKTransitTypeAir;
	
You can add a BarCode.

    request.AddBarCode("01927847623423234234", BarcodeType.PKBarcodeFormatPDF417, "UTF-8", "01927847623423234234");

Finally, generate the pass by passing the request into the instance of the Generator. This will create the signed manifest and package all the the image files into zip.

    byte[] generatedPass = generator.Generate(request);

If you are using ASP.NET MVC for example, you can return this byte[] as a Passbook package

	return new FileContentResult(generatedPass, "application/vnd.apple.pkpass");
 
##Updating passes

To be able to update your pass, you must provide it with a callback. When generating your request, you must provide it with an AuthenticationToken and a WebServiceUrl.

	request.AuthenticationToken = "vxwxd7J8AlNNFPS8k0a0FfUFtq0ewzFdc";
    request.WebServiceUrl = "http://192.168.1.59:82/api";

There are several methods that the Pass will invoke when it's installed and updated. To see a reference implementation of this, look at the PassRegistrationController class in the Passbook.Sample.Web project.

The method that is of most interest in the beginning is the Post method as this actually captures the PushToken for the passes. The UpdateController has a very simple mechanism for sending an update. At present, the device ID is hard-coded, but this should provide a working reference

## Sample Web Application

Included as part of the solution, the Passbook.Sample.Web project allows you to create some sample passes. You can run this and access the pages from your iPhone to see how the passes are installed and to see the registration and update mechanism in operation.

The project also includes some dummy requests, so illustrate how you can create wrappers around the basic PassGenerationRequest. The above BoardPass can be generated using the BoardingPassGeneratorRequest. Instead of adding the fields explicitly, this encapsulates this logic, so you can call 

	request.Origin = "San Francisco";
    request.OriginCode = "SFO";

    request.Destination = "London";
    request.DestinationCode = "LDN";

    request.Seat = "7A";
    request.BoardingGate = "F12";
    request.PassengerName = "John Appleseed";

    request.TransitType = TransitType.PKTransitTypeAir;

/Home/Index will open a simple HTML page where you can choose the card type.  
/Pass/EventTicket will generate an event based Pass (not fully functional).  
/Pass/BoardingPass will generate simple baording card.

These passes are functional and can be saved Passbook.

##NuGet

Dotnet-passbook is also available to  download from NuGet.

	Install-Package dotnet-passbook

##Contribute
All pull requests are welcomed! If you come across an issue you cannot fix, please raise an issue or drop me an email at tomas@tomasmcguinness.com or follow me on twitter @tomasmcguinness

##PassVerse	
PassVerse is a service that I am building that will offer a simple way to design and generate Passbook passes. It will allow you to track usage and offer an API to push updates to your passes. This will provide you with an alternative to implementing this technology yourself. You can register your interest at [www.passverse.com](http://www.passverse.com)

##License

Dotnet-passbook is distributed under the MIT license: [http://tomasmcguinness.mit-license.org/](http://tomasmcguinness.mit-license.org/)
