//
//  Encoder.c
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

#include <ctype.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "Encoder.h"

void makeSmartPassword(char *str);

#define kCodeLen 12

#define MIN(X,Y) ((X) < (Y) ? (X) : (Y))

#define D(i)   (dmp[(m[i]+o)%10 + 0 ])
#define L(i)   (dmp[(m[i]+o)%25 + 10])
#define LV(i)  (dmp[(m[i]+o)%5  + 10])  // j and y are not treated as vocals
#define U(i)   (dmp[(m[i]+o)%24 + 35])
#define UV(i)  (dmp[(m[i]+o)%3  + 35])  // J and Y are not treated as vocals
#define P(i)   (dmp[(m[i]+o)%9  + 59])
#define M(i)   (dmp[(m[i]+o)%49 + 10])
#define DL(i)  (dmp[(m[i]+o)%35 + 0 ])
#define DM(i)  (dmp[(m[i]+o)%59 + 0 ])
unsigned char dmp[] = {    
    '0','1','2','3','4','5','6','7','8','9',    // 10
    'a','e','i','o','u','b','c','d','f','g',    // 20
    'h','j','k','m','n','p','q','r','s','t',    // 30 
    'v','w','x','y','z','A','E','U','B','C',    // 40   ('O' and 'I' are left out in favour of 0 and 1)
    'D','F','G','H','J','K','L','M','N','P',    // 50
    'Q','R','S','T','V','W','X','Y','Z','.',    // 60
    '!','?','#','&','@','*','+','='             // 68
};

// hash algorithm based on Robert Sedgewick
uint32_t hash(char* str)
{
    if (str == NULL) return 0;
    
    uint32_t b    = 378551;
    uint32_t a    = 63689;
    uint32_t hash = 0;
    uint32_t i    = 0;
    
    for(i = 0;i < strlen(str);i++) {
        hash = hash * a + abs(str[i]);
        a = a * b;
    }
    
    return hash;
}

/*
 Generates a four digit number by taking the hash of the given string and projects into into a integer space 
 between 1000 and 9999. In case of an empty string, a random four digit number is generated.
 */
uint32_t generateFourDigitNumber(char* str)
{
    uint32_t h;
    if (str == NULL || str[0] == 0) {
        char rnd[] = { random()%10,random()%10,random()%10,random()%10,random()%10,0 };
        h = hash(rnd);
    } 
    else
        h = hash(str);
    
    return h % 9000 + 1000;
}

/*
 Takes a usernamen/email (login), a master password (pwd), an urls/zip/or whatever (hint) and format options
 to generate a password phrase and copies into the str.

 Important: The string have to be ASCII encoded! Convert UTF-8, UTF-16, ISO-8859, ... to ASCII (without extended)
 See http://www.asciitable.com/
 
 Example used below: login = "abcd", pwd = "bbcd", hint = "efgh" options = 8/letters and digits/lower
 */
void encode(char *str,char* login,char* pwd,char* hint,int len,SymbolsType symbolsType,LetterCaseType letterCase,int smartpwd) 
{
    // projects the big value domains of login, pwd and hint into a much smaller value domain
    // "abcd" -> 6480, "bbcd" -> 9369, "efgh" -> 7616 
    uint32_t user = generateFourDigitNumber(login);
    uint32_t master = generateFourDigitNumber(pwd);
    uint32_t site = generateFourDigitNumber(hint);
    
    // constructs a twelve character string based on the three hashes
    // [6480], [9369], [7616] -> "936964807616"
    char seed[kCodeLen+1];
    sprintf(seed,"%u%u%u",user,master,site);
    
    // generates an array of eight two digit numbers, based on the hashes - always builds the number by
    // taking the digits of two different hashes
    // "6480 9369 7616" -> [69], [43], [86], [09], [97], [36], [61], [96], [x], [x], [x], [x]
    uint32_t m[kCodeLen];
    for (int i = 0;i<8;i++) {
        // takes first digit and converts character into number (char-48)
        // multiplies it by 10 to make it the decimal part and adds the second digit
        m[i] = (seed[i] - 48)*10 + seed[i+4] - 48;
    }
    // the last four numbers are generated as sum of the just calculated eight numbers
    // with that approach, the master influences all digits
    // "6480 9369 7616" -> [69], [43], [86], [09], [97], [36], [61], [96], [66], [79], [47], [05]
    for (int i = 0;i<4;i++) {
        // takes first digit and converts character into number (char-48)
        // multiplies it by 10 to make it the decimal part and adds the second digit
        m[i+8] = (m[i] + m[i+4])%100;
    }
    
    // a slight change in login, pwd or site results in a different offset into the lookup table
    // hash("648093697616")%75 -> 32
    uint32_t o = hash(seed) % 75;
    char code[kCodeLen+1];
    code[kCodeLen] = 0;
    
    // composites the password according to the options
    // there are basically four types of symbols: digits, lower, upper and punctuations
    // the password is construced with three groups of four characters. each group consists of at allowed symbol types
    switch (symbolsType) {
        case Digits: {
            char c[] = { D(0),D(3),D(6),D(9), D(1),D(4),D(7),D(10), D(2),D(5),D(8),D(11),0 };
            strcpy(code,c);
        }
            break;
        case Letters: {
            switch (letterCase) {
                case Lower: {
                    char c[] = { L(0),LV(3),L(6),L(9), L(1),LV(4),L(7),L(10), L(2),LV(5),L(8),L(11),0 };
                    strcpy(code,c);
                }
                    break;
                case Upper: {
                    char c[] = { U(0),UV(3),U(6),U(9), U(1),UV(4),U(7),U(10), U(2),UV(5),U(8),U(11),0 };
                    strcpy(code,c);
                }
                    break;
                case Mixed: {
                    char c[] = { M(0),LV(3),U(6),M(9), M(1),L(4),UV(7),M(10), M(2),L(5),U(8),M(11),0 };
                    strcpy(code,c);
                }
                    break;
            } 
            break;
        }
        case DigitsAndLetters: {
            switch (letterCase) {
                case Lower: {
                    char c[] = { DL(0),D(3),L(6),LV(9), DL(1),D(4),L(7),LV(10), DL(2),D(5),L(8),LV(11),0 };
                    strcpy(code,c);
                }
                    break;
                case Upper: {
                    char c[] = { U(0),D(3),UV(6),D(9), U(1),D(4),UV(7),D(10), U(2),D(5),UV(8),D(11),0 };
                    strcpy(code,c);
                }
                    break;
                case Mixed: {
                    char c[] = { DM(0),LV(3),U(6),D(9), DM(1),L(4),UV(7),D(10), DM(2),LV(5),U(8),D(11),0 };
                    strcpy(code,c);
                }
                    break;
            } break;
        }
        case DigitsAndPunctuation: {
            char c[] = { D(0),P(3),D(6),D(9), D(1),P(4),D(7),D(10), D(2),P(5),D(8),D(11),0 };            
            strcpy(code,c);
        }
            break;
        case LettersAndPunctuation: {
            switch (letterCase) {
                case Lower: {
                    char c[] = { L(0),P(3),LV(6),L(9), L(1),P(4),LV(7),L(10), L(2),P(5),LV(8),L(11),0 };
                    strcpy(code,c);
                }
                    break;
                case Upper: {                    
                    char c[] = { U(0),P(3),UV(6),U(9), U(1),P(4),UV(7),U(10), U(2),P(5),UV(8),U(11),0 };
                    strcpy(code,c);
                }
                    break;
                case Mixed: {
                    char c[] = { M(0),U(3),LV(6),P(9), M(1),UV(4),L(7),P(10), M(2),U(5),LV(8),P(11),0 };
                    strcpy(code,c);
                }
                    break;                    
            } 
            break;
        }
        case DigitsAndLettersAndPunctuation: {
            switch (letterCase) {
                case Lower: {
                    char c[] = { DL(0),D(3),LV(6),P(9), DL(1),D(4),L(7),P(10), DL(2),D(5),L(8),P(11),0 };                    
                    strcpy(code,c);
                }
                    break;
                case Upper: {
                    char c[] = {  U(0),D(3),P(6),UV(9), UV(1),D(4),P(7),U(10), D(2),D(5),P(8),UV(11),0 };
                    strcpy(code,c);
                }
                    break;
                case Mixed: {
                    char c[] = { L(0),D(3),UV(6),P(9), LV(1),D(4),U(7),P(10), LV(2),D(5),U(8),P(11),0 };
                    strcpy(code,c);
                }
                    break;            
            } 
            break;
        }
            
    }
    
    // shuffles the three blocks individually w1se 59ou o8yi -> 1sew 59ou 8yio, because h[] = { 1 0 1 }
    // with that, a small change in user, pwd or site has a big impact to the code
    uint32_t h[] = { (master+site)%4, (user+site)%4, (user+master)%4 };
    char shuffled[kCodeLen+1];
    shuffled[kCodeLen] = 0;
    for (int r=0; r<3; r++) {
        for (int i=0; i<4; i++) {
            shuffled[r*4+i] = code[r*4+(i+h[r])%4];
        }
    }
    
    // "w1se59ouo8yi" -> "1sew59ou8yio"
    shuffled[MIN(len,kCodeLen)] = 0;
    
    // groups password characters in a way the password gets more memorizable
    // "w1se59ouo8yi" -> "w1se59ou"
    if (smartpwd) makeSmartPassword(shuffled);
    
    strcpy(str,shuffled);
}

// comparer for bubble sort
int comesAfter(char c1,char c2) {
    if (isdigit(c1) && isalpha(c2)) return 1;
    if (ispunct(c1) && isalpha(c2)) return 1;
    if (ispunct(c1) && isdigit(c2)) return 1;
    return 0;
}

#define SWAP(c1,c2) { char temp; temp = c1; c1 = c2; c2 = temp; } 

void bubbleSort(char *str,int len) 
{ 
    int last = len - 2; 
    int isChanged = 1; 
    
    while (last >= 0 && isChanged) { 
        isChanged = 0; 
        for (int k = 0;k <= last;k++ ) {
            if (comesAfter(str[k],str[k+1])) { 
                SWAP(str[k],str[k+1]);
                isChanged = 1; 
            } 
        }
        last--; 
    } 
} 

int isvocal(char c) 
{
    if (isalpha(c)) {
        char lc = tolower(c);
        if (lc == 'a' || lc == 'e' || lc == 'i' || lc == 'o' || lc == 'u') return 1;
    }
    return 0;
}

int isconsonant(char c)
{
    if (isalpha(c) && !isvocal(c)) return 1;
    return 0;
}

void makeSmartPassword(char *str)
{
    int len = strlen(str);
    bubbleSort(str,len);
    
    // puts one punctuation character ahead of the string if there is one at the end
    if (ispunct(str[len-1])) {
        char first = str[len-1];
        for (int index = len-2; index>=0; index--) {
            str[index+1] = str[index];
        }
        str[0] = first;
    }
    
    // if in a sequence c1,c2,c3 c1 and c2 are both vocals or both none-vocals, then c2 and c3 are swapped
    for (int run = 0; run < 3; run++) {        
        for (int index = 0;index < len-2;index++) {
            if (isalpha(str[index+2])) {
                if (isvocal(str[index]) && isvocal(str[index+1]))
                    SWAP(str[index+1],str[index+2]);
                if (isconsonant(str[index]) && isconsonant(str[index+1]))
                    SWAP(str[index+1],str[index+2]);
            }
        }
    }
}




















