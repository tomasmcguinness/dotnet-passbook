dotnet-passbook
===============

A .Net Library for generating Passbook packages for iOS 6

The goal of this project is to build a library that can generate, sign and zip Passbook files for use with iOS 6's Passbook. 

To run the generator, you need to have a Passbook certificate, which you get from the Developer Portal. You must have a developer account. There are 
instructions on my blog for generating a certificate if you're using a Windows Machine. If you generate the certificate on OS X, you'll need to export 
and import it to your Windows machine. When doing so, please ensure you export the Private key. Just specify the Thumbnail of your certificate in the Request 
you're generating. It will search your "Current User" certificate store.

There are two types of request available at the moment. EventPassGeneratorRequest and StoreCardGeneratorRequest. These will have all the appropriate fields
for the type of pass you're trying to create. Eventually I'll add Coupon and BoardingPass requests.

At this time, the Tests project doesn't do anything, so just run the Web project.

/Home/Index will open a simple HTML page where you can choose the card type.
/Pass/Event will generate an event based Pass (not fully functional)
/Pass/StoreCard will generate a Starbucks sample card. This is working and the card will be saved to the iPhone's Passbook.
