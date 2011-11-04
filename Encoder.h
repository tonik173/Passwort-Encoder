//
//  Encoder.h
//  Passwort
//
//  Created by Kaufmann Toni on 24.08.11.
//  Copyright (c) 2011 n3xd software studios ag.
//
// 	Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// 	and associated documentation files (the "Software"), to deal in the Software without 
// 	restriction, including without limitation the rights to use, copy, merge, publish, 
// 	distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the 
//	Software is furnished to do so, subject to the following conditions:
// 	The above copyright notice and this permission notice shall be included in
// 	all copies or substantial portions of the Software.
// 	
// 	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
// 	BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// 	NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// 	DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// 	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//  Use this code to generate the same passwords in your application as “Passwort“ does.
//

typedef enum {
    Digits = 1,
    Letters = 2,
    DigitsAndLetters = 3,
    DigitsAndPunctuation = 4,
    LettersAndPunctuation = 5,
    DigitsAndLettersAndPunctuation = 6
} SymbolsType;

typedef enum {
    Lower = 1,
    Upper = 2,
    Mixed = 3
} LetterCaseType;

void encode(char *str,char* login,char* pwd,char* hint,int len,SymbolsType symbolsType,LetterCaseType letterCase,int smartpwd); 
void makeSmartPassword(char *str);
