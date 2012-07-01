dotnet-passbook
===============

A .Net Library for generating Passbook packages for iOS 6

The goal of this project is to build a library that can generate, sign and zip Passbook files for use with iOS 6's Passbook. 

To run the generator, you need to have a passbook certificate. There are instructions on my blog on generating a certificate.

To generate a pass, just run the web application and access http://server/pass - This creates a simple Event package with just a title.

Issues:
Whilst the pass is generated and iOS doesn't record any errors, it doesn't appear in the application passbook. I'm looking into this issue.
