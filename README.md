#dotnet-passbook
A .Net Library for generating Passbook packages for iOS 6

##Why
The goal of this project is to build a library that can generate, sign and zip Passbook files for use with iOS 6's Passbook. 

##Technical Stuff
Before you run the PassGenerator, you need to have the installed your Passbook certificate, which you get from the Developer Portal. You must have a full iPhone developer account. There are [instructions on my blog](http://www.tomasmcguinness.com/2012/06/28/generating-an-apple-ios-certificate-using-windows/) for generating a certificate if you're using a Windows Machine.

When you install the certificate, be sure to note the certificate's Thumbprint. 

To generate a pass, start by declaring a PassGenerator.

    PassGenerator generator = new PassGenerator();

Next, create a PassGeneratorRequest of the type you want. At present, EventPassGeneratorRequest and StorePassGeneratorRequest are available. Fill out all the mandatory values.

    EventPassGeneratorRequest request = new EventPassGeneratorRequest();
    request.Identifier = "pass.tomsamcguinness.events";
    request.CertThumbprint = "22C5FADDFBF935E26E2DDB8656010C7D4103E02E";
    request.CertLocation = System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser;
    request.FormatVersion = 1;
    request.SerialNumber = "121212";
    request.Description = "My first pass";
    request.OrganizationName = "Tomas McGuinness";
    request.TeamIdentifier = "Team America";
    request.LogoText = "My Pass";
    request.BackgroundColor = "#FFFFFF";
    request.ForegroundColor = "#000000";

Next, define the background images you with to use. You must always include both standard and retina sized images.

    request.BackgroundFile = @"C:/Icons/Starbucks/background.png";
    request.BackgroundRetinaFile = @"C:/Icons/Starbucks/background@2x.png";

    request.IconFile =@"C:/Icons/icon.png";
    request.IconRetinaFile = @"C:/Icons/icon@2x.png";

    request.LogoFile = @"C:/Icons/logo.png";
    request.LogoRetinaFile = @"C:/Icons/logo@2x.png";

You can now provide more pass specific information

    request.EventName = "Jeff Wayne's War of the Worlds";
    request.VenueName = "The O2";

Finally, you can add a BarCode.

    request.AddBarCode("01927847623423234234", BarcodeType.PKBarcodeFormatPDF417, "UTF-8", "01927847623423234234");

Generate the pass, by passing the request into the Generator. This will actually create the package in a temporary folder and sign it.

    Pass generatedPass = generator.Generate(request);

To get to contents of the package. This will return a byte[] representing all the data in the signed zipfile. 

	generatedPass.GetPackage()

##Updating passes

To be able to update your pass, you must provide it with a callback. When generating your request, you must provide it with an AuthentionToken and a WebServiceUrl.

	request.AuthenticationToken = "vxwxd7J8AlNNFPS8k0a0FfUFtq0ewzFdc";
    request.WebServiceUrl = "http://192.168.1.59:82/api";

There are several methods that the Pass will invoke when it's installed and updated. To see a reference implementation of this, look at the PassRegistrationController class in the Passbook.Sample.Web project.

The method that is of most interest in the beginning is the Post method as this actually captures the PushToken for the passes. The UpdateController has a very simple mechanism for sending an update. At present, the device ID is hard-coded, but this should provide a working reference

## Sample Web

As part of Passbook.Sample.Web project, you can run this and access the pages from your iPhone to see how the passes are installed and to see the registration and update mechanism is progress.

/Home/Index will open a simple HTML page where you can choose the card type.
/Pass/Event will generate an event based Pass (not fully functional)
/Pass/StoreCard will generate a Starbucks sample card. This is working and the card will be saved to the iPhone's Passbook.

<h2>Contribute</h2>
All pull requests are welcomed! If you come across an issue you cannot fix, please raise an issue or drop me an email at tomas@tomasmcguinness.com or follow me on twitter @tomasmcguinness
