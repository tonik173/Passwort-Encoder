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
using System.Xml;

namespace n3xd.Passwort.EncoderTest {

   public class TestEncoder {


      public static List<TestData> ReadTestDataFromXML(string strUrl) {
         XmlTextReader reader = null;
         List<TestData> list = new List<TestData>();

         string elemName = "";
         try {
            reader = new XmlTextReader( strUrl );
            while(reader.Read()) {

               if(reader.NodeType == XmlNodeType.Element) {
                  elemName = reader.Name;

                  if(elemName.Equals( "dict" )) {
                     TestData testData = ReadTestData( reader );
                     list.Add( testData );
                  }
               }
            }
         } catch(Exception exc) {
            Console.WriteLine("RunAndTest(): Fehler beim Parsen: \r\n" + exc.Message );
         } finally {
            if(reader != null) {
               reader.Close();
            }
         }
         return list;
      }

      public static string[] ReadNextElem(XmlReader reader) {
         string[] retVal = new string[2];
         int found = 0;   // 1 = foundKey 2 = foundValue

         while(found < 2 && reader.Read()) {
            if(reader.NodeType == XmlNodeType.Element) {      
               if(reader.Name.Equals( "key" )) {
                  retVal[0] = reader.ReadString();
                  found = 1;
               } else if(reader.Name.Equals( "integer" ) || reader.Name.Equals( "string" )) {
                  retVal[1] = reader.ReadString();
                  found = 2;
               }
            }
         }
         return retVal;
  
      }

      public static TestData ReadTestData(XmlReader reader){
         TestData testData = new TestData();
         string elemName;
         string keyValue;
         string valValue;

         while(reader.Read() && !reader.Name.Equals("dict")) {

            string[] keyVal = ReadNextElem( reader );
            if(keyVal[0].Equals( "Username" )) {
               testData.UserLogin = keyVal[1];
               break;

            } else if(keyVal[0].Equals( "Hint" )) {
               testData.Hint = keyVal[1];

            } else if(keyVal[0].Equals( "Master" )) {
               testData.MasterPwd = keyVal[1];

            } else if(keyVal[0].Equals( "Symbols" )) {
               testData.SetSymbolType(keyVal[1]);

            } else if(keyVal[0].Equals( "Case" )) {
               testData.SetLetterCaseType(keyVal[1]);

            } else if(keyVal[0].Equals( "Length" )) {
               testData.CodeLength = int.Parse(keyVal[1]);

            } else if(keyVal[0].Equals( "SmartPasswords" )) {
               testData.SetSmartPasswords(keyVal[1]);

            } else if(keyVal[0].Equals( "Code" )) {
               testData.GeneratedPwd = keyVal[1];
            } 
         }

         return testData;
      }

      public static int ReadTextAsInt32(string elemName, XmlTextReader reader) {
         int retVal = -1;
         string tmpString = null;
         reader.Read();
         if(reader.NodeType != XmlNodeType.Text) {
            Console.WriteLine( "ReadTextAsInt32(): <" + elemName + ">: kein Text gefunden!" );
            
         } else {
            try {
               tmpString = reader.ReadContentAsString();
               retVal = Convert.ToInt32( tmpString );
            } catch(Exception ex) {
               try {
                  bool tryBool = Convert.ToBoolean( tmpString );
                  retVal = Convert.ToInt32( tryBool );
               } catch(Exception) {

               }
               if(retVal == -1) {
                  Console.WriteLine( "ReadTextAsInt32(): <" + elemName + ">: " + ex );
                  throw ex;
               }
            }
         }
         return retVal;
      }

   }
}
