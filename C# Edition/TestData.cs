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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using n3xd.Passwort.PwEncode;

namespace n3xd.Passwort.EncoderTest {

   public class TestData {

      private string _userLogin;                                  // "Username" im XML
      private string _hint;                                       // "Hint" im XML (site, url)
      private string _masterPwd;                                  // "Master" im XML

      private CodeCharacterBase.SymbolsType _symbolType;          // "Symbols" im XML
      private CodeCharacterBase.LetterCaseType _letterCaseType;   // "Case" im XML
      private int _codeLength;                                    // "Length" im XML
      private bool _smartPasswords;                               // "SmartPasswords" im XML

      private string _generatedPwd;                               // "Code" im XML


      public override string ToString() {
         return "  SYMBOL = " + _symbolType +
                "\r\n  LETTER = " + _letterCaseType +
                "\r\n  LEN = " + _codeLength +
                "\r\n  SMARTPW = " + _smartPasswords +
                "\r\n  LOGIN = " + _userLogin +
                "\r\n  HINT = " + _hint +
                "\r\n  MASTERPW = " + _masterPwd;

      }

      #region properties

      public string UserLogin {
         get {
            return _userLogin;
         }
         set {
            _userLogin = value;
         }
      }

      public CodeCharacterBase.SymbolsType SymbolType {
         get {
            return _symbolType;
         }
      }

      public void SetSymbolType(string strSymolType) {
         // Symbol table: Digits = 1, Letters = 2, DigitsAndLetters = 3, DigitsAndPunctuation = 4,
         // LettersAndPunctuation = 5, DigitsAndLettersAndPunctuation = 6 

         if(strSymolType.Equals("1")){
            _symbolType = CodeCharacterBase.SymbolsType.Digits;
         } else if(strSymolType.Equals( "2" )) {
            _symbolType = CodeCharacterBase.SymbolsType.Letters;
         } else if(strSymolType.Equals( "3" )) {
            _symbolType = CodeCharacterBase.SymbolsType.DigitsAndLetters;
         } else if(strSymolType.Equals( "4" )) {
            _symbolType = CodeCharacterBase.SymbolsType.DigitsAndPunctuation;
         } else if(strSymolType.Equals( "5" )) {
            _symbolType = CodeCharacterBase.SymbolsType.LettersAndPunctuation;
         } else if(strSymolType.Equals( "6" )) {
            _symbolType = CodeCharacterBase.SymbolsType.DigitsAndLettersAndPunctuation;
         }
      }

      public bool SmartPasswords {
         get {
            return _smartPasswords;
         }
      }

      public void SetSmartPasswords(string strSmartPw) {
         _smartPasswords = strSmartPw.Equals("1");         
      }

      public string MasterPwd {
         get {
            return _masterPwd;
         }
         set {
            _masterPwd = value;
         }
      }

      public int CodeLength {
         get {
            return _codeLength;
         }
         set {
            _codeLength = value;
         }
      }

      public string Hint {
         get {
            return _hint;
         }
         set {
            _hint = value;
         }
      }

      public string GeneratedPwd {
         get {
            return _generatedPwd;
         }
         set {
            _generatedPwd = value;
         }
      }

      public CodeCharacterBase.LetterCaseType LetterCaseType {
         get {
            return _letterCaseType;
         }
      }

      public void SetLetterCaseType(string strLetterCase) {
         if(strLetterCase.Equals( "1" )) {
            _letterCaseType = CodeCharacterBase.LetterCaseType.Lower;
         } else if(strLetterCase.Equals( "2" )) {
            _letterCaseType = CodeCharacterBase.LetterCaseType.Upper;
         } else if(strLetterCase.Equals( "3" )) {
            _letterCaseType = CodeCharacterBase.LetterCaseType.Mixed;
         }
      }

      #endregion properties

   }
}
