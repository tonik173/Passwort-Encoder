//
//  Program.cs
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
   class Program {
      static void Main(string[] args) {

         //TestPwEncode_KommentarBeispiel();
         TestPwEncode();

         Console.ReadLine();
      }

      public static void TestPwEncode_KommentarBeispiel() {
         CodeCharacterBase ccb = new CodeCharacterBase();
         PwEncoding enc = new PwEncode.PwEncoding( ccb, "bbcd", 
                                                   CodeCharacterBase.SymbolsType.DigitsAndLetters,
                                                   CodeCharacterBase.LetterCaseType.Lower,
                                                   12, false );
         string genPw = enc.Encode( "abcd", "efgh" );
         Console.WriteLine( "Base-Test: excpected=9pibm6pa, generated=" + genPw );
      }


      public static void TestPwEncode() {
         CodeCharacterBase ccb = new CodeCharacterBase();
         int failCounter = 0;
         List<TestData> list = TestEncoder.ReadTestDataFromXML( @"..\..\..\TestData\refdata-1000.xml" );
         int i = 1;
         foreach(var data in list) {
            if(true ){ // && data.SymbolType.Equals(CodeCharacterBase.SymbolsType.DigitsAndLettersAndPunctuation)) {
               if(i == 39) {
                  bool stop = true;
               }

               
               PwEncoding enc = new PwEncode.PwEncoding( ccb, data.MasterPwd, data.SymbolType, data.LetterCaseType,
                                                         data.CodeLength, data.SmartPasswords );
               string genPw = enc.Encode( data.UserLogin, data.Hint );

               string ok = "ok";
               if(!genPw.Equals( data.GeneratedPwd )) {
                  ok = "NO";
                  failCounter++;
               }
               Console.WriteLine( i.ToString( "D3" ) + ". Test: " + ok + " -excpected=" +
                                  data.GeneratedPwd.PadRight( 12, ' ' ) + " generated=" + genPw );
               if(!genPw.Equals( data.GeneratedPwd )) {
                  Console.WriteLine( data );
               }
               i++;
            }
         }
         Console.WriteLine( "FailCounter = " + failCounter );
      }
   }
}
