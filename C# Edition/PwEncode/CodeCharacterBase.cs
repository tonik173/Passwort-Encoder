//  Passwort
//
//  Created by Christian Häfeli, Octobre 2011.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace n3xd.Passwort.PwEncode {

   /// <summary>
   /// Defines the code-character set for the encoding and the way its 
   /// accessed depending on a SymbolTye and a LetterCaseType. This is 
   /// done by selectros (IndexPositionData) which indicates the starting
   /// position (Offset) and the length (ModValue) in the code-character
   /// set.
   /// 
   /// The GetEncoding() - method returns an encoding string based on an
   /// array of two-digits numbers (and SymbolTye and a LetterCaseType).
   /// 
   /// By the SetSelector() - method the wiring could also be changed after
   /// initialisation.
   /// </summary>
   public class CodeCharacterBase {

      public enum SymbolsType {
         Digits = 1,
         Letters = 2,
         DigitsAndLetters = 3,
         DigitsAndPunctuation = 4,
         LettersAndPunctuation = 5,
         DigitsAndLettersAndPunctuation = 6
      }

      public enum LetterCaseType {
         None = 0,
         Lower = 1,
         Upper = 2,
         Mixed = 3
      }

      public struct IndexPositionData {
         int modValue;
         int offset;

         public int ModValue {
            get {
               return modValue;
            }
            set {
               modValue = value;
            }
         }
         
         public int Offset {
            get {
               return offset;
            }
            set {
               offset = value;
            }
         }
        
         public IndexPositionData(int mod, int off){
            modValue = mod;
            offset = off;
         }
      }

      private char[] _characterBase = new char[]{
         '0','1','2','3','4','5','6','7','8','9', //  0- 9
         'a','e','i','o','u','b','c','d','f','g', // 10-19
         'h','j','k','m','n','p','q','r','s','t', // 20-29
         'v','w','x','y','z','A','E','U','B','C', // 30-39
         'D','F','G','H','J','K','L','M','N','P', // 40-49
         'Q','R','S','T','V','W','X','Y','Z','.', // 50-59
         '!','?','#','&','@','*','+','='          // 60-68

      };

      private IndexPositionData D = new IndexPositionData( 10, 0 );  // digits
      private IndexPositionData L = new IndexPositionData( 25, 10 ); // lower letters only 
      private IndexPositionData LV = new IndexPositionData( 5, 10 ); // letters upper only vocals
      private IndexPositionData U = new IndexPositionData( 24, 35 ); // upper letters only
      private IndexPositionData UV = new IndexPositionData( 3, 35 ); // upper letters only vocals
      private IndexPositionData P = new IndexPositionData( 9, 59 );  // punctuations
      private IndexPositionData M = new IndexPositionData( 49, 10 ); // mixed letters
      private IndexPositionData DL = new IndexPositionData( 35, 0 ); // digits and lower letters
      private IndexPositionData DM = new IndexPositionData( 59, 0 ); // digits and mixed letters


      private int _maxCodeLength = 12;
      private int[] _indexSelection = new int[] { 0, 3, 6, 9, 1, 4, 7, 10, 2, 5, 8, 11 };
      private Hashtable _htSelectors = new Hashtable();

      /// <summary>
      /// The Selectors are definded and wire-up in the constructor.
      /// </summary>
      public CodeCharacterBase() {
         
         IndexPositionData[] ipd;
         // Digits
         ipd = new IndexPositionData[]{ D,D,D,D,  D,D,D,D,  D,D,D,D};
         SetSelector( SymbolsType.Digits, LetterCaseType.None, ipd );

         // Lower letters
         ipd = new IndexPositionData[] { L,LV,L,L,  L,LV,L,L,  L,LV,L,L };
         SetSelector( SymbolsType.Letters, LetterCaseType.Lower, ipd );

         // Upper letters
         ipd = new IndexPositionData[] { U,UV,U,U,  U,UV,U,U,  U,UV,U,U };
         SetSelector( SymbolsType.Letters, LetterCaseType.Upper, ipd );

         // Mixed letters 
         ipd = new IndexPositionData[] { M,LV,U,M,  M,L,UV,M,  M,L,U,M };
         SetSelector( SymbolsType.Letters, LetterCaseType.Mixed, ipd );

         // Digits and lower letters 
         ipd = new IndexPositionData[] { DL,D,L,LV,  DL,D,L,LV,  DL,D,L,LV };
         SetSelector( SymbolsType.DigitsAndLetters, LetterCaseType.Lower, ipd );

         // Digits and upper letters 
         ipd = new IndexPositionData[] { U,D,UV,D,  U,D,UV,D,  U,D,UV,D };
         SetSelector( SymbolsType.DigitsAndLetters, LetterCaseType.Upper, ipd );

         // Digits and mixed letters
         ipd = new IndexPositionData[] { DM,LV,U,D,  DM,L,UV,D,  DM,LV,U,D };
         SetSelector( SymbolsType.DigitsAndLetters, LetterCaseType.Mixed, ipd );

         // Digits and punctuation
         ipd = new IndexPositionData[] { D,P,D,D,  D,P,D,D,  D,P,D,D };
         SetSelector( SymbolsType.DigitsAndPunctuation, LetterCaseType.None, ipd );

         // Lower letters and punctuation
         ipd = new IndexPositionData[] { L,P,LV,L,  L,P,LV,L,  L,P,LV,L };
         SetSelector( SymbolsType.LettersAndPunctuation, LetterCaseType.Lower, ipd );

         // Upper letters and punctuation
         ipd = new IndexPositionData[] { U,P,UV,U,  U,P,UV,U,  U,P,UV,U };
         SetSelector( SymbolsType.LettersAndPunctuation, LetterCaseType.Upper, ipd );

         // Mixed letters and punctuation
         ipd = new IndexPositionData[] { M,U,LV,P,  M,UV,L,P,  M,U,LV,P };
         SetSelector( SymbolsType.LettersAndPunctuation, LetterCaseType.Mixed, ipd );

         // Digits and lower letters and punctuation
         ipd = new IndexPositionData[] { DL,D,LV,P,  DL,D,L,P,  DL,D,L,P };
         SetSelector( SymbolsType.DigitsAndLettersAndPunctuation, LetterCaseType.Lower, ipd );

         // Digits and upper letters and punctuation
         ipd = new IndexPositionData[] { U,D,P,UV,  UV,D,P,U,  D,D,P,UV };
         SetSelector( SymbolsType.DigitsAndLettersAndPunctuation, LetterCaseType.Upper, ipd );

         // Digits and mixed letters and punctuation
         ipd = new IndexPositionData[] { L,D,UV,P,  LV,D,U,P,  LV,D,U,P };
         SetSelector( SymbolsType.DigitsAndLettersAndPunctuation, LetterCaseType.Mixed, ipd );

      }

      public int MaxCodeLength {
         get {
            return _maxCodeLength;
         }
         set {
            _maxCodeLength = value;
         }
      }

      /// <summary>
      /// This methods allows to define the way the code-character set is accessed by a
      /// given SymbolType and a LetterCaseType.
      /// </summary>
      /// <param name="symbol">{digits, letters, ...}</param>
      /// <param name="letterCase">{lower, upper, mixed}</param>
      /// <param name="ipdArr">startposition and range</param>
      public void SetSelector(SymbolsType symbol, LetterCaseType letterCase, IndexPositionData[] ipdArr) {
         string key = symbol.ToString() + letterCase.ToString();
         if(_htSelectors.ContainsKey( key )) {
            _htSelectors[key] =  ipdArr;
         } else {
            _htSelectors.Add( key,  ipdArr );
         }
      }

      /// <summary>
      /// For each two-digits number in the the given adr-arry a character out of the
      /// code-character set is selected - depending on the given SymolsType and LetterCaseType.
      /// 
      /// The returning encoding-string is built by adding each of the characters to a string.
      /// 
      /// </summary>
      /// <param name="adr">two-digits int-array</param>
      /// <param name="symbol">{digits, letters, ...}</param>
      /// <param name="letterCase">{lower, upper, mixed}</param>
      /// <returns>encoding - string</returns>
      public string GetEncoding(int[] adr, SymbolsType symbol, LetterCaseType letterCase) {
         string key = symbol.ToString() + letterCase.ToString();
         IndexPositionData[] ipdArr = (IndexPositionData[])_htSelectors[key];
         
         StringBuilder retStr = new StringBuilder();
         for(int i = 0; i < adr.Length; i++) {
            IndexPositionData ipd = ipdArr[i];
            int dmpIndex = adr[_indexSelection[i]] % ipd.ModValue + ipd.Offset;
            char c = _characterBase[dmpIndex];
            retStr.Append( c );
         }
         return retStr.ToString();
      }
   }
}
