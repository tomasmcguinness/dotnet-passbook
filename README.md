# dotnet-passbook

A .NET Standard Library for generating Passbook packages for iOS Wallet (formerly Passbook)

[![.NET Core](https://github.com/tomasmcguinness/dotnet-passbook/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/tomasmcguinness/dotnet-passbook/actions/workflows/dotnetcore.yml)

## Installing

dotnet-passbook is also available to download from NuGet

	Install-Package dotnet-passbook

## Why

Creating passes for Apple's Passbook is pretty simple, but requires the use of PKI for signing manifest files, which isn't so simple! During the course of building PassVerse.com (no longer available), I created a .Net library that performs all the steps. I decided to open source this library for other .NET developers.

## Requirements

The solution requires Visual Studio 2017 or higher. The library is built for .NET Standard 2.0.

## Certificates

Before you run the PassGenerator, you need to ensure you have all the necessary certificates installed. There are two required.

Firstly, you need to install the Apple WWDR (WorldWide Developer Relations) certificate. You can download that from here [http://www.apple.com/certificateauthority/](http://www.apple.com/certificateauthority/) . You must install it in the Local Machine's Intermediate Certificate store. I've hardcoded that location

Secondly, you need to installed your Passbook certificate, which you get from the Developer Portal. You must have a full iPhone developer account. There are [instructions on my blog](http://www.tomasmcguinness.com/2012/06/28/generating-an-apple-ios-certificate-using-windows/) for generating a certificate if you're using a Windows Machine.

You can place this certificate in any of the stores, but it must be placed into the "personal" folder.  When constructing the request for the pass, you specify the location and thumbprint for the certificate. If running this code in IIS for example, installing the certificate in the Local Machine area might make access easier. Alternatively, you could place the certificate into the AppPool's user's certification repository. When you install the certificate, be sure to note the certificate's Thumbprint.

## Technical Stuff

To generate a pass, start by declaring a PassGenerator.

    PassGenerator generator = new PassGenerator();

Next, create a PassGeneratorRequest. This is a raw request that gives you the full power to add all the fields necessary for the pass you wish to create. Each pass is broken down into several sections. Each section is rendered in a different way, based on the style of the pass you are trying to produce. For more information on this, please consult Apple's Passbook Programming Guide. The example below here will show how to create a very basic boarding pass.

Since each pass has a set of mandatory data, fill that in first.

    PassGeneratorRequest request = new PassGeneratorRequest();
    request.PassTypeIdentifier = "pass.tomsamcguinness.events";   
    request.TeamIdentifier = "RW121242";
    request.SerialNumber = "121212";
    request.Description = "My first pass";
    request.OrganizationName = "Tomas McGuinness";
    request.LogoText = "My Pass";

Colours can be specified in HTML format or in RGB format.

    request.BackgroundColor = "#FFFFFF";
    request.LabelColor = "#000000";
    request.ForegroundColor = "#000000";
    
    request.BackgroundColor = "rgb(255,255,255)";
    request.LabelColor = "rgb(0,0,0)";
    request.ForegroundColor = "rgb(0,0,0)";

To select the certificate there are two options. Firstly, you can use the Windows Certificate store to hold the certificates. You choose the location of your Passbook certificate by specifying the thumbprint of the certificates. 

    request.CertThumbprint = "22C5FADDFBF935E26E2DDB8656010C7D4103E02E";
    request.CertLocation = System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser;

An alternative way to pass the certificates into the PassGenerator is to load them as byte[] and add them to the request.

    byte[] certData = <the contents of the certificate>; // e.g. File.ReadAllBytes(@"C:\path\to\your\certificate");
    request.Certificate = certData;
    request.CertificatePassword = "abc123"; // The password for the certificate's private key.

The Apple WWDRC Certificate can only be loaded as a byte[].

    var certData =  <the contents of the WWDR certificate>; // e.g. File.ReadAllBytes(@"C:\path\to\the\apple\wwdr\certificate");
    request.AppleWWDRCACertificate = certData;

Next, define the images you with to use. You must always include both standard and retina sized images. Images are supplied as byte[].

    // override icon and icon retina
    request.Images.Add(PassbookImage.Icon, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon.png")));
    request.Images.Add(PassbookImage.Icon2X, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon@2x.png")));
    request.Images.Add(PassbookImage.Icon3X, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon@3x.png")));

You can now provide more pass specific information. The Style must be set and then all information is then added to fields to the required sections. For a baording pass, the fields are add to three sections;  primary, secondary and auxiliary.

	request.Style = PassStyle.BoardingPass;

    request.AddPrimaryField(new StandardField("origin", "San Francisco", "SFO"));
    request.AddPrimaryField(new StandardField("destination", "London", "LDN"));

    request.AddSecondaryField(new StandardField("boarding-gate", "Gate", "A55"));

    request.AddAuxiliaryField(new StandardField("seat", "Seat", "G5" ));
    request.AddAuxiliaryField(new StandardField("passenger-name", "Passenger", "Thomas Anderson"));

    request.TransitType = TransitType.PKTransitTypeAir;

You can add a BarCode.

    request.AddBarCode(BarcodeType.PKBarcodeFormatPDF417, "01927847623423234234", "ISO-8859-1", "01927847623423234234");

Starting with iOS 9, multiple barcodes are now supported. This helper method supports this new feature. If you wanted to support iOS 8 and earlier, you can use the method SetBarcode().

To link the pass to an existing app, you can add the app's Apple ID to the AssociatedStoreIdentifiers array.

    request.AssociatedStoreIdentifiers.Add(551768478);

Finally, generate the pass by passing the request into the instance of the Generator. This will create the signed manifest and package all the the image files into zip.

    byte[] generatedPass = generator.Generate(request);

If you are using ASP.NET MVC for example, you can return this byte[] as a Passbook package

    return new FileContentResult(generatedPass, "application/vnd.apple.pkpass");
	
### Troubleshooting Passes

If the passes you create don't seem to open on iOS or in the simulator, the payload is probably invalid. To aid troubleshooting, I've created this simple tool - https://pkpassvalidator.azurewebsites.net - just run your pkpass file through this and it might give some idea what's wrong. The tool is new (Jul'18) and doesn't check absolutely everything. I'll try and add more validation to the generator itself.

### IIS Security
	
If you're running the signing code within an IIS application, you might run into some issues accessing the private key of your certificates.  To resolve this open MMC => Add Certificates (Local computer) snap-in => Certificates (Local Computer) => Personal => Certificates => Right click the certificate of interest => All tasks => Manage private key => Add IIS AppPool\AppPoolName and grant it Full control. Replace "AppPoolName" with the name of the application pool that your app is running under. (sometimes IIS_IUSRS)

## Updating passes

To be able to update your pass, you must provide it with a callback. When generating your request, you must provide it with an AuthenticationToken and a WebServiceUrl. Both of these values are required. The WebServiceUrl must be HTTPS by default, but you can disable this requirement in the iOS developer options on any device you're testing on.

The authentication token is a string that will be included in the header of all requets made to your API. It's your responsibility to validate this token.

    request.AuthenticationToken = "<a secret to ensure authorized access>";
    request.WebServiceUrl = "https://<your api>";

The webservice you point to must support Apple's protocol, outlined here https://developer.apple.com/library/archive/documentation/PassKit/Reference/PassKit_WebService/WebService.html#//apple_ref/doc/uid/TP40011988

I'm working on a sample implementation of the protocol in ASP.Net Core and you can find it on the branch new-sample-webservice.

## NFC Support

As of version 2.0.1, the NFC keys are now supported. To use them, just set hte Nfc property with a new Nfc object. Both the message and encoded public key values are mandatory.

	 PassGeneratorRequest request = new PassGeneratorRequest();
         request.Nfc = new Nfc("THE NFC Message", "<encoded private key>");
	 
I cannot supply any information as to the values required since it's not available publically.

## Contribute

All pull requests are welcomed! If you come across an issue you cannot fix, please raise an issue or drop me an email at tomas@tomasmcguinness.com or follow me on twitter @tomasmcguinness

## Supporting the project

If you use dotnet-passbook, please consider donating https://www.buymeacoffee.com/fMKJ2NnQ3 - it will enable me to keep the project updated with improvements and changes. Thanks!

## License

Dotnet-passbook is distributed under the MIT license: [http://tomasmcguinness.mit-license.org/](http://tomasmcguinness.mit-license.org/)
