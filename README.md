# dotnet-passbook

A .NET Standard Library for generating Passbook packages for iOS Wallet (formerly Passbook)

[![.NET Core](https://github.com/tomasmcguinness/dotnet-passbook/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/tomasmcguinness/dotnet-passbook/actions/workflows/dotnetcore.yml)

## Supporting the project

If you use dotnet-passbook, please consider buying me a cup of coffee

<a href='https://ko-fi.com/G2G11TQK5' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://cdn.ko-fi.com/cdn/kofi2.png?v=3' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

## Installing

dotnet-passbook is also available to download from NuGet

```ps
Install-Package dotnet-passbook
```

## Why

Creating passes for Apple's Passbook is pretty simple, but requires the use of PKI for signing manifest files, which isn't so simple! During the course of building PassVerse.com (no longer available), I created a .Net library that performs all the steps. I decided to open source this library for other .NET developers.

## Requirements

The solution requires Visual Studio 2017 or higher. The library is built for .NET Standard 2.0.

## Certificates

Before you run the PassGenerator, you need to ensure you have all the necessary certificates installed. There are two required.

Firstly, you need to your Passbook certificate, which you get from the Developer Portal. You must have an iOS developer account.

Secondly, you need the Apple WWDR (WorldWide Developer Relations) certificate. You can download that from here [http://www.apple.com/certificateauthority/](http://www.apple.com/certificateauthority/).

Depending on when you generated your Passbook certificate you'll need either the G1 or G4 certificate. If you generated your passbook certificate on or before the January 27, 2022, use G1. Otherwise use G4.

The other "Worldwide Developer Relations" certificates listed here won't work with Apple Wallet. *("Sorry, your Pass cannot be installed to Passbook at this time.")*

There are [instructions on my blog](http://www.tomasmcguinness.com/2012/06/28/generating-an-apple-ios-certificate-using-windows/) for generating a certificate using IIS if you're using a Windows machine

If you're on Linux/macOS or would prefer to use OpenSSL on Windows, check out [using-openssl.md](using-openssl.md) for instructions on how to create the necessary certificates using OpenSSL.

When moving certificates around, be sure that your Passbook certificate always includes the private key component, else the signing will fail.

## Technical Stuff

To generate a pass, start by declaring a PassGenerator.

```cs
PassGenerator generator = new PassGenerator();
```

Next, create a PassGeneratorRequest. This is a raw request that gives you the full power to add all the fields necessary for the pass you wish to create. Each pass is broken down into several sections. Each section is rendered in a different way, based on the style of the pass you are trying to produce. For more information on this, please consult Apple's Passbook Programming Guide. The example below here will show how to create a very basic boarding pass.

Since each pass has a set of mandatory data, fill that in first.

```cs
PassGeneratorRequest request = new PassGeneratorRequest();
request.PassTypeIdentifier = "pass.tomsamcguinness.events";   
request.TeamIdentifier = "RW121242";
request.SerialNumber = "121212";
request.Description = "My first pass";
request.OrganizationName = "Tomas McGuinness";
request.LogoText = "My Pass";
```

Colours can be specified in HTML format or in RGB format.

```cs
request.BackgroundColor = "#FFFFFF";
request.LabelColor = "#000000";
request.ForegroundColor = "#000000";

request.BackgroundColor = "rgb(255,255,255)";
request.LabelColor = "rgb(0,0,0)";
request.ForegroundColor = "rgb(0,0,0)";
```

You must provide both the Apple WWDR and your Passbook certificate as X509Certificate instances. NOTE: This is a change from previous versions.

```cs
request.AppleWWDRCACertificate = new X509Certificate(...);
request.PassbookCertificate = new X509Certificate(...);
```

When you are creating the X509Certificate instances in the cloud, you may experience issues with the signing process. I recommend you use the MachineKeySet and Exportable X509KeyStorageFlags.

```cs
X509KeyStorageFlags flags = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
X509Certificate2 certificate = new X509Certificate2(bytes, password, flags);
```

Next, define the images you with to use. You must always include both standard and retina sized images. Images are supplied as byte[].

```cs
request.Images.Add(PassbookImage.Icon, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon.png")));
request.Images.Add(PassbookImage.Icon2X, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon@2x.png")));
request.Images.Add(PassbookImage.Icon3X, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon@3x.png")));
```

You can now provide more pass specific information. The Style must be set and then all information is then added to fields to the required sections. For a boarding pass, the fields are add to three sections;  primary, secondary and auxiliary.

```cs
request.Style = PassStyle.BoardingPass;

request.AddPrimaryField(new StandardField("origin", "San Francisco", "SFO"));
request.AddPrimaryField(new StandardField("destination", "London", "LDN"));

request.AddSecondaryField(new StandardField("boarding-gate", "Gate", "A55"));

request.AddAuxiliaryField(new StandardField("seat", "Seat", "G5" ));
request.AddAuxiliaryField(new StandardField("passenger-name", "Passenger", "Thomas Anderson"));

request.TransitType = TransitType.PKTransitTypeAir;
```

You can add a BarCode.

```cs
request.AddBarcode(BarcodeType.PKBarcodeFormatPDF417, "01927847623423234234", "ISO-8859-1", "01927847623423234234");
```

Starting with iOS 9, multiple barcodes are now supported. This helper method supports this new feature. If you wanted to support iOS 8 and earlier, you can use the method SetBarcode().

To link the pass to an existing app, you can add the app's Apple ID to the AssociatedStoreIdentifiers array.

```cs
request.AssociatedStoreIdentifiers.Add(551768478);
```

Finally, generate the pass by passing the request into the instance of the Generator. This will create the signed manifest and package all the the image files into zip.

```cs
byte[] generatedPass = generator.Generate(request);
```

If you are using ASP.NET MVC for example, you can return this byte[] as a Passbook package

```cs
return new FileContentResult(generatedPass, "application/vnd.apple.pkpass");
```

iOS 15 introduced the ability to bundle and distribute multiple passes using a singular .pkpasses file. You can generate pass bundles as well by passing in a dictionary of requests values and string keys that represent the filename for each individual request.

```cs
PassGeneratorRequest myFirstRequest = new PassGeneratorRequest();
PassGeneratorRequest mySecondRequest = new PassGeneratorRequest();

// Build out your requests

List<PassGeneratorRequest> requests = new List<PassGeneratorRequest>;

requests.Add(myFirstRequest);
request.Add(mySecondRequest);

byte[] generatedBundle = generator.Generate(requests);
```

The resulting byte array is treated almost identically to a singular `.pkpass` file, but with a different extension and MIME type (*pkpasses*)

```cs
return new FileContentResult(generatedBundle, "application/vnd.apple.pkpasses")
{
    FileDownloadName = "tickets.pkpasses.zip"
};
```

### Troubleshooting Passes

If the passes you create don't seem to open on iOS or in the simulator, the payload is probably invalid. To aid troubleshooting, I've created this simple tool - https://pkpassvalidator.azurewebsites.net - just run your `pkpass` file through this and it might give some idea what's wrong. The tool is new (Jul'18) and doesn't check absolutely everything. I'll try and add more validation to the generator itself.

### IIS Security

If you're running the signing code within an IIS application, you might run into some issues accessing the private key of your certificates.  To resolve this open MMC => Add Certificates (Local computer) snap-in => Certificates (Local Computer) => Personal => Certificates => Right click the certificate of interest => All tasks => Manage private key => Add IIS AppPool\AppPoolName and grant it Full control. Replace "AppPoolName" with the name of the application pool that your app is running under. (sometimes `IIS_IUSRS`)

## Updating passes

To be able to update your pass, you must provide it with a callback. When generating your request, you must provide it with an AuthenticationToken and a WebServiceUrl. Both of these values are required. The WebServiceUrl must be HTTPS by default, but you can disable this requirement in the iOS developer options on any device you're testing on.

The authentication token is a string that will be included in the header of all requests made to your API. It's your responsibility to validate this token.

```cs
request.AuthenticationToken = "<a secret to ensure authorized access>";
request.WebServiceUrl = "https://<your api>";
```

The web service you point to must support Apple's protocol, outlined in the [PassKit Web Service Reference](https://developer.apple.com/library/archive/documentation/PassKit/Reference/PassKit_WebService/WebService.html#//apple_ref/doc/uid/TP40011988)

## NFC Support

As of version 2.0.1, the NFC keys are now supported. To use them, just set the Nfc property with a new NFC object. Both the message and encoded public key values are mandatory.

```cs
PassGeneratorRequest request = new PassGeneratorRequest();
request.Nfc = new Nfc("THE NFC Message", "<encoded private key>");
```

Unfortunately, I cannot supply any information as to the values required since it's not available publically. If anyone knows what goes here, I'd be more than happy to add changes to my library to support this key.

## Contribute

All pull requests are welcomed! If you come across an issue you cannot fix, please raise an issue or drop me an email at tomas@tomasmcguinness.com or follow me on twitter @tomasmcguinness

## License

dotnet-passbook is distributed under the MIT license: [http://tomasmcguinness.mit-license.org/](http://tomasmcguinness.mit-license.org/)
