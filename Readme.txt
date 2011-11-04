****************************************************************************************************
About
****************************************************************************************************
The ”Passwort“ app takes the web site name, the username and a master password to calculate a password formatted according your settings. With the same set of input values (web site, username, master password and format options), the app will always regenerate same passwords. Since the master password is your secret, no one else can reproduce your passwords.


****************************************************************************************************
Links
****************************************************************************************************
Facebook product-site: www.facebook.com/passwort.enApp Store: itunes.apple.com/us/app/passwort/id474416620?ls=1&mt=8]****************************************************************************************************
Native, C Edition as used in the Passwort App
****************************************************************************************************
Encoder.h - header file for c version of generateor algorithm 
Encoder.c - the passwort generator algorithm in c


****************************************************************************************************
C# Edition by Christian Häfeli
****************************************************************************************************
Download C# Edition Folder. This directory contains the source code for the encoder as well as
an application for running the test data.


****************************************************************************************************
Test data
****************************************************************************************************
Important: The Passwort App passes the three string parameters with ASCII encoding. It converts diacritical letters, such as umlauts into their base character (e.g. å, ä, â -> a). The test data file do contain diacritical characters. This is on intention in order to make sure, the conversion to base character works.

refdata-10.xml - 10 testcases
refdata-100.xml - 100 testcases
refdata-100000.xml - 100000 testcases

Each test case is stored as a key/value pair. A single test case is

<dict>
	<!-- Case sensitiy: Lower = 1, Upper = 2, Mixed = 3 -->
	<key>Case</key>
	<integer>1</integer>

	<!-- Generated password -->
	<key>Code</key>
	<string>*uzixotuc?</string>

	<!-- Any string, usually a url -->
	<key>Hint</key>
	<string>qFUa*zUG7[QlD</string>

	<!-- Generated password length -->
	<key>Length</key>
	<integer>10</integer>

	<!-- Master password -->
	<key>Master</key>
	<string>6h/</string>
	
	<!-- Smart passwords: 0 = NO, 1 = YES -->
	<key>SmartPasswords</key>
	<integer>1</integer>

	<!-- Symbol table: Digits = 1, Letters = 2, DigitsAndLetters = 3, DigitsAndPunctuation = 4,
    		LettersAndPunctuation = 5, DigitsAndLettersAndPunctuation = 6 -->
	<key>Symbols</key>
	<integer>5</integer>

	<!-- Username -->
	<key>Username</key>
	<string>zc[z-sUr/py]E!Pj!SIP|e00p:</string>

</dict>

