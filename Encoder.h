//
//  Encoder.h
//  Passwort
//
//  Created by Kaufmann Toni on 24.08.11.
//  Copyright 2011 n3xd ag. All rights reserved.
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
