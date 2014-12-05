using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SI_tema1_EAS
{
    class Program
    {
        //git test
        static string[,] state, cypherKey;


        static string[,] Sbox = new string[16, 16] 
                 {  // populate the Sbox matrix
          /* 0     1   2    3     4    5    6   7    8    9     a   b    c    d    e    f */
    /*0*/  {"63","7c","77","7b","f2","6b","6f","c5","30","01","67","2b","fe","d7","ab","76"},
    /*1*/  {"ca","82","c9","7d","fa","59","47","f0","ad","d4","a2","af","9c","a4","72","c0"},
    /*2*/  {"b7","fd","93","26","36","3f","f7","cc","34","a5","e5","f1","71","d8","31","15"},
    /*3*/  {"04","c7","23","c3","18","96","05","9a","07","12","80","e2","eb","27","b2","75"},
    /*4*/  {"09","83","2c","1a","1b","6e","5a","a0","52","3b","d6","b3","29","e3","2f","84"},
    /*5*/  {"53","d1","00","ed","20","fc","b1","5b","6a","cb","be","39","4a","4c","58","cf"},
    /*6*/  {"d0","ef","aa","fb","43","4d","33","85","45","f9","02","7f","50","3c","9f","a8"},
    /*7*/  {"51","a3","40","8f","92","9d","38","f5","bc","b6","da","21","10","ff","f3","d2"},
    /*8*/  {"cd","0c","13","ec","5f","97","44","17","c4","a7","7e","3d","64","5d","19","73"},
    /*9*/  {"60","81","4f","dc","22","2a","90","88","46","ee","b8","14","de","5e","0b","db"},
    /*a*/  {"e0","32","3a","0a","49","06","24","5c","c2","d3","ac","62","91","95","e4","79"},
    /*b*/  {"e7","c8","37","6d","8d","d5","4e","a9","6c","56","f4","ea","65","7a","ae","08"},
    /*c*/  {"ba","78","25","2e","1c","a6","b4","c6","e8","dd","74","1f","4b","bd","8b","8a"},
    /*d*/  {"70","3e","b5","66","48","03","f6","0e","61","35","57","b9","86","c1","1d","9e"},
    /*e*/  {"e1","f8","98","11","69","d9","8e","94","9b","1e","87","e9","ce","55","28","df"},
    /*f*/  {"8c","a1","89","0d","bf","e6","42","68","41","99","2d","0f","b0","54","bb","16"} 
       };


    static string[,]iSbox=new string[16,16]{
        /* 0     1   2    3     4    5    6   7    8    9     a   b    c    d    e    f */
    /*0*/{"52","09","6a","d5","30","36","a5","38","bf","40","a3","9e","81","f3","d7","fb"},
    /*1*/{"7c","e3","39","82","9b","2f","ff","87","34","8e","43","44","c4","de","e9","cb"},
    /*2*/{"54","7b","94","32","a6","c2","23","3d","ee","4c","95","0b","42","fa","c3","4e"},
    /*3*/{"08","2e","a1","66","28","d9","24","b2","76","5b","a2","49","6d","8b","d1","25"},
    /*4*/{"72","f8","f6","64","86","68","98","16","d4","a4","5c","cc","5d","65","b6","92"},
    /*5*/{"6c","70","48","50","fd","ed","b9","da","5e","15","46","57","a7","8d","9d","84"},
    /*6*/{"90","d8","ab","00","8c","bc","d3","0a","f7","e4","58","05","b8","b3","45","06"},
    /*7*/{"d0","2c","1e","8f","ca","3f","0f","02","c1","af","bd","03","01","13","8a","6b"},
    /*8*/{"3a","91","11","41","4f","67","dc","ea","97","f2","cf","ce","f0","b4","e6","73"},
    /*9*/{"96","ac","74","22","e7","ad","35","85","e2","f9","37","e8","1c","75","df","6e"},
    /*a*/{"47","f1","1a","71","1d","29","c5","89","6f","b7","62","0e","aa","18","be","1b"},
    /*b*/{"fc","56","3e","4b","c6","d2","79","20","9a","db","c0","fe","78","cd","5a","f4"},
    /*c*/{"1f","dd","a8","33","88","07","c7","31","b1","12","10","59","27","80","ec","5f"},
    /*d*/{"60","51","7f","a9","19","b5","4a","0d","2d","e5","7a","9f","93","c9","9c","ef"},
    /*e*/{"a0","e0","3b","4d","ae","2a","f5","b0","c8","eb","bb","3c","83","53","99","61"},
    /*f*/{"17","2b","04","7e","ba","77","d6","26","e1","69","14","63","55","21","0c","7d"}
    };

        static byte[,] Rcon = new byte[4, 10] {{0x01, 0x02,0x04,0x08,0x10,0x20,0x40,0x80,0x1b,0x36},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}
        };





        static void addRoundKey(string[,] state, string[,] cypherKey)
        {
            int first, second, result;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    first = Convert.ToInt32(state[j, i], 16);
                    second = Convert.ToInt32(cypherKey[j, i], 16);
                    result = first ^ second;
                    state[j, i] = result.ToString("X");
                }
        }



        static void subBytes(string[,] state, string[,] Sbox)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    string elem = state[i, j];
                    char x, y;
                    int x2, y2;
                    x = elem[0];
                    x2 = x;
                    if (!(x2 >= 48 && x2 <= 57))
                        switch (x)
                        {
                            case 'A': x2 = 10; break;
                            case 'B': x2 = 11; break;
                            case 'C': x2 = 12; break;
                            case 'D': x2 = 13; break;
                            case 'E': x2 = 14; break;
                            case 'F': x2 = 15; break;
                            case 'a': x2 = 10; break;
                            case 'b': x2 = 11; break;
                            case 'c': x2 = 12; break;
                            case 'd': x2 = 13; break;
                            case 'e': x2 = 14; break;
                            case 'f': x2 = 15; break;
                            default: break;
                        }
                    else x2 = (int)Char.GetNumericValue(x);

                    if (state[i, j].Length > 1)
                    {
                        y = elem[1];
                        y2 = y;
                        if (!(y2 >= 48 && y2 <= 57))
                            switch (y)
                            {
                                case 'A': y2 = 10; break;
                                case 'B': y2 = 11; break;
                                case 'C': y2 = 12; break;
                                case 'D': y2 = 13; break;
                                case 'E': y2 = 14; break;
                                case 'F': y2 = 15; break;
                                case 'a': y2 = 10; break;
                                case 'b': y2 = 11; break;
                                case 'c': y2 = 12; break;
                                case 'd': y2 = 13; break;
                                case 'e': y2 = 14; break;
                                case 'f': y2 = 15; break;
                                default: break;
                            }
                        else y2 = (int)Char.GetNumericValue(y);
                    }
                    else
                    {
                        y2 = x2;
                        x2 = 0;
                    }

                    state[i, j] = Sbox[x2, y2];


                }
        }

        static void invSubBytes(string[,] state, string[,] Sbox)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    string elem = state[i, j];
                    char x, y;
                    int x2, y2;
                    x = elem[0];
                    x2 = x;
                    if (!(x2 >= 48 && x2 <= 57))
                        switch (x)
                        {
                            case 'A': x2 = 10; break;
                            case 'B': x2 = 11; break;
                            case 'C': x2 = 12; break;
                            case 'D': x2 = 13; break;
                            case 'E': x2 = 14; break;
                            case 'F': x2 = 15; break;
                            case 'a': x2 = 10; break;
                            case 'b': x2 = 11; break;
                            case 'c': x2 = 12; break;
                            case 'd': x2 = 13; break;
                            case 'e': x2 = 14; break;
                            case 'f': x2 = 15; break;
                            default: break;
                        }
                    else x2 = (int)Char.GetNumericValue(x);

                    if (state[i, j].Length > 1)
                    {
                        y = elem[1];
                        y2 = y;
                        if (!(y2 >= 48 && y2 <= 57))
                            switch (y)
                            {
                                case 'A': y2 = 10; break;
                                case 'B': y2 = 11; break;
                                case 'C': y2 = 12; break;
                                case 'D': y2 = 13; break;
                                case 'E': y2 = 14; break;
                                case 'F': y2 = 15; break;
                                case 'a': y2 = 10; break;
                                case 'b': y2 = 11; break;
                                case 'c': y2 = 12; break;
                                case 'd': y2 = 13; break;
                                case 'e': y2 = 14; break;
                                case 'f': y2 = 15; break;
                                default: break;
                            }
                        else y2 = (int)Char.GetNumericValue(y);
                    }
                    else
                    {
                        y2 = x2;
                        x2 = 0;
                    }

                    state[i, j] = iSbox[x2, y2];


                }
        }


        static void shiftRows(string[,] state) {
            string first, second, third;

            first = state[1, 0];
            for (int i = 0; i < 3; i++)
                state[1, i] = state[1, i+1];
            state[1, 3] = first;

            first = state[2, 0];
            second = state[2, 1];
            state[2,0]=state[2,2];
            state[2,1]=state[2,3];
            state[2,2]=first;
            state[2,3]=second;

            first = state[3, 0];
            second = state[3, 1];
            third = state[3, 2];
            state[3, 0] = state[3,3];
            state[3, 1] = first;
            state[3, 2] = second;
            state[3, 3] = third;


        }

        static void invShiftRows(string[,] state)
        {
            string first, second, third,forth;

            first = state[1, 0];
            second = state[1, 1];
            third = state[1, 2];
            forth = state[1, 3];
            state[1, 3] = third;
            state[1, 2] = second;
            state[1, 1] = first;
            state[1, 0] = forth;


            first = state[2, 3];
            second = state[2, 2];
            state[2, 3] = state[2, 1];
            state[2, 2] = state[2, 0];
            state[2, 1] = first;
            state[2, 0] = second;

            first = state[3, 0];
            second = state[3, 1];
            third = state[3, 2];
            forth = state[3, 3];
            state[3, 3] = first;
            state[3, 2] = forth;
            state[3, 1] = third;
            state[3, 0] = second;
        }


        static byte xorby02(byte x){
            if (x < 0x80)
                return (byte)(int)(x << 1);
            else return (byte)((int)(x<<1)^(int)(0x1b));
        }

        static byte xorby03(byte x) {
            return (byte)((int)xorby02(x) ^ (int)x);
        }


        private static byte xorby09(byte b)
        {
            return (byte)((int)xorby02(xorby02(xorby02(b))) ^(int)b);
        }

        private static byte xorby0b(byte b)
        {
            return (byte)((int)xorby02(xorby02(xorby02(b))) ^(int)xorby02(b) ^(int)b);
        }

        private static byte xorby0d(byte b)
        {
            return (byte)((int)xorby02(xorby02(xorby02(b))) ^(int)xorby02(xorby02(b)) ^(int)(b));
        }

        private static byte xorby0e(byte b)
        {
            return (byte)((int)xorby02(xorby02(xorby02(b))) ^(int)xorby02(xorby02(b)) ^(int)xorby02(b));
        }

        static void mixColumns(string[,] state) {
            string[,] temp;
            temp = new string[4, 4];
            for(int i=0;i<4;i++)
            {
                temp[0, i] = (xorby02((Convert.ToByte(state[0, i], 16))) ^ xorby03(Convert.ToByte(state[1, i], 16)) ^ (Convert.ToByte(state[2, i], 16)) ^ (Convert.ToByte(state[3, i], 16))).ToString("X");
                temp[1, i] = ((Convert.ToByte(state[0, i], 16)) ^ xorby02(Convert.ToByte(state[1, i], 16)) ^ xorby03((Convert.ToByte(state[2, i], 16))) ^ (Convert.ToByte(state[3, i], 16))).ToString("X");
                temp[2, i] = ((Convert.ToByte(state[0, i], 16)) ^ (Convert.ToByte(state[1, i], 16)) ^ xorby02((Convert.ToByte(state[2, i], 16))) ^ xorby03((Convert.ToByte(state[3, i], 16)))).ToString("X");
                temp[3, i] = (xorby03((Convert.ToByte(state[0, i], 16))) ^ (Convert.ToByte(state[1, i], 16)) ^ (Convert.ToByte(state[2, i], 16)) ^ xorby02(Convert.ToByte(state[3, i], 16))).ToString("X");
            }
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    state [i,j]= temp[i,j];
        }

        static void invMixColumns(string[,] state)
        {
            string[,] temp;
            temp = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                temp[0, i] = (xorby0e((Convert.ToByte(state[0, i], 16))) ^ xorby0b(Convert.ToByte(state[1, i], 16)) ^ xorby0d(Convert.ToByte(state[2, i], 16)) ^ xorby09(Convert.ToByte(state[3, i], 16))).ToString("X");
                temp[1, i] = (xorby09(Convert.ToByte(state[0, i], 16)) ^ xorby0e(Convert.ToByte(state[1, i], 16)) ^ xorby0b((Convert.ToByte(state[2, i], 16))) ^ xorby0d(Convert.ToByte(state[3, i], 16))).ToString("X");
                temp[2, i] = (xorby0d(Convert.ToByte(state[0, i], 16)) ^ xorby09(Convert.ToByte(state[1, i], 16)) ^ xorby0e((Convert.ToByte(state[2, i], 16))) ^ xorby0b((Convert.ToByte(state[3, i], 16)))).ToString("X");
                temp[3, i] = (xorby0b((Convert.ToByte(state[0, i], 16))) ^ xorby0d(Convert.ToByte(state[1, i], 16)) ^ xorby09(Convert.ToByte(state[2, i], 16)) ^ xorby0e(Convert.ToByte(state[3, i], 16))).ToString("X");
            }
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    state[i, j] = temp[i, j];
        }


       static void keySchedule(string[,] cypherKey){
            string xx;
            string [,] temp = new string[4,4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4;j++ )
                    temp[i,j] = cypherKey[i,j];
            xx = temp[0, 3];
            temp[0, 3] = temp[1, 3];
            temp[1, 3] = temp[2, 3];
            temp[2, 3] = temp[3, 3];
            temp[3, 3] = xx;

        for(int i=0;i<4;i++){
            string elem = temp[i, 3];
                    char x, y;
                    int x2, y2;
                    x = elem[0];
                    x2 = x;
                    if (!(x2 >= 48 && x2 <= 57))
                        switch (x)
                        {
                            case 'A': x2 = 10; break;
                            case 'B': x2 = 11; break;
                            case 'C': x2 = 12; break;
                            case 'D': x2 = 13; break;
                            case 'E': x2 = 14; break;
                            case 'F': x2 = 15; break;
                            case 'a': x2 = 10; break;
                            case 'b': x2 = 11; break;
                            case 'c': x2 = 12; break;
                            case 'd': x2 = 13; break;
                            case 'e': x2 = 14; break;
                            case 'f': x2 = 15; break;
                            default: break;
                        }
                    else x2 = (int)Char.GetNumericValue(x);

                    if (temp[i, 3].Length > 1)
                    {
                        y = elem[1];
                        y2 = y;
                        if (!(y2 >= 48 && y2 <= 57))
                            switch (y)
                            {
                                case 'A': y2 = 10; break;
                                case 'B': y2 = 11; break;
                                case 'C': y2 = 12; break;
                                case 'D': y2 = 13; break;
                                case 'E': y2 = 14; break;
                                case 'F': y2 = 15; break;
                                case 'a': y2 = 10; break;
                                case 'b': y2 = 11; break;
                                case 'c': y2 = 12; break;
                                case 'd': y2 = 13; break;
                                case 'e': y2 = 14; break;
                                case 'f': y2 = 15; break;
                                default: break;
                            }
                        else y2 = (int)Char.GetNumericValue(y);
                    }
                    else
                    {
                        y2 = x2;
                        x2 = 0;
                    }

                    temp[i, 0] = Sbox[x2, y2];
                    
        }


        temp[0, 0] = (Convert.ToByte(cypherKey[0, 0], 16) ^ Convert.ToByte(temp[0, 0], 16) ^ Rcon[0, 0]).ToString("X");
        temp[1, 0] = (Convert.ToByte(cypherKey[1, 0], 16) ^ Convert.ToByte(temp[1, 0], 16) ^ Rcon[1, 0]).ToString("X");
        temp[2, 0] = (Convert.ToByte(cypherKey[2, 0], 16) ^ Convert.ToByte(temp[2, 0], 16) ^ Rcon[2, 0]).ToString("X");
        temp[3, 0] = (Convert.ToByte(cypherKey[3, 0], 16) ^ Convert.ToByte(temp[3, 0], 16) ^ Rcon[3, 0]).ToString("X");


        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 9;j++ )
                Rcon[i,j]=Rcon[i,j+1];

                for (int j = 1; j < 4; j++)
                    for (int i = 0; i < 4; i++)
                        temp[i, j] = (Convert.ToByte(cypherKey[i, j], 16) ^ Convert.ToByte(temp[i, j - 1], 16)).ToString("X");


   // cypherKey = temp;

    for (int i = 0; i < 4; i++)
        for (int j = 0; j < 4; j++)
            cypherKey[i, j] = temp[i, j];



        } //sfarsit keySchedule

      static void writeOutput() {
           //scriem in fisier
         string  path = Directory.GetCurrentDirectory();
           path = System.IO.Directory.GetParent(path).FullName;
           path = System.IO.Directory.GetParent(path).FullName;
           path = path + @"\iesire.txt";
           var sw = new StreamWriter(path, true);
           for (int i = 0; i < 4; i++)
               for (int j = 0; j < 4; j++)
               {
                   sw.Write(state[j, i] + " ");
               }
         //  sw.WriteLine("");
           sw.Close();
       }


      static void encrypt() {

          addRoundKey(state, cypherKey);
          for (int i = 1; i <= 9; i++)
          {
              subBytes(state, Sbox);
              shiftRows(state);
              mixColumns(state);
              keySchedule(cypherKey);
              addRoundKey(state, cypherKey);
          }
          subBytes(state, Sbox);
          shiftRows(state);
          keySchedule(cypherKey);
          addRoundKey(state, cypherKey);
      
      }

      static void decrypt()
      {
          string [,] originalKey=new string[4,4];
            for(int i=0;i<4;i++)  
                for(int j=0;j<4;j++)
                    originalKey[i,j]=cypherKey[i,j];

          int ksn=10; //key schedule number 

          Rcon = new byte[4, 10] {{0x01, 0x02,0x04,0x08,0x10,0x20,0x40,0x80,0x1b,0x36},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}};

          for(int i=1;i<=ksn;i++)
              keySchedule(cypherKey);
          ksn--;

          addRoundKey(state, cypherKey);

          for (int k = 1; k <= 9; k++)
          {
              invShiftRows(state);
              invSubBytes(state, Sbox);
                  for (int i = 0; i < 4; i++)
                      for (int j = 0; j < 4; j++)
                          cypherKey[i, j] = originalKey[i, j];

                Rcon= new byte[4,10] {{0x01, 0x02,0x04,0x08,0x10,0x20,0x40,0x80,0x1b,0x36},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}};

                  for(int i=1;i<=ksn;i++)
                    keySchedule(cypherKey);
                  ksn--;
              addRoundKey(state, cypherKey);
              invMixColumns(state);


          }
          invShiftRows(state);
          invSubBytes(state, Sbox);
                  for (int i = 0; i < 4; i++)
                      for (int j = 0; j < 4; j++)
                          cypherKey[i, j] = originalKey[i, j];

                   for(int i=1;i<=ksn;i++)
                    keySchedule(cypherKey);
                  ksn--;
          addRoundKey(state, cypherKey);

      }

      static void Cypher() {
          string[,] originalKey = new string[4, 4];
          //construim pathul de intrare:
          string path = Directory.GetCurrentDirectory();
          path = System.IO.Directory.GetParent(path).FullName;
          path = System.IO.Directory.GetParent(path).FullName;
          path = path + @"\intrare.txt";
          var sr = new StreamReader(path);
          string plainText = sr.ReadLine();
          string key = sr.ReadLine();
          char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

          //facem la fel si cu matricea cheie
          cypherKey = new string[4, 4];
          string[] keyChars = key.Split(delimiterChars);
          string[] chars = plainText.Split(delimiterChars);
          int i = 0, j = 0;
          i = 0;
          j = 0;
          foreach (string s in keyChars)
          {
              cypherKey[j, i] = s;
              j++;
              if (j == 4)
              {
                  j = 0;
                  i++;
              }
          }


          //construim matricea state
          state = new string[4, 4];
          i = 0;
          j = 0;
          int ok = 1;

          foreach (string s in chars)
          {
              if (i != 4)
              {
                  state[j, i] = s;
                  j++;
                  if (j == 4)
                  {
                      j = 0;
                      i++;
                  }
              }
              else
              {

                  for (int q = 0; q < 4; q++)
                      for (int z = 0; z < 4; z++)
                          originalKey[q, z] = cypherKey[q, z];

                  encrypt();
                  writeOutput();
                  for (int k = 0; k < 4; k++)
                      for (int l = 0; l < 4; l++)
                          state[k, l] = "00";
                  i = 0;
                  j = 0;
                  ok = 0;
              }

              if (ok == 0)
              {
                  ok = 1;
                  if (i != 4)
                  {
                      state[j, i] = s;
                      j++;
                      if (j == 4)
                      {
                          j = 0;
                          i++;
                      }
                  }
              }
          }
          if (ok == 1)
          {
              for (int q = 0; q < 4; q++)
                  for (int z = 0; z < 4; z++)
                     // cypherKey[q, z] = originalKey[q, z];

              Rcon = new byte[4, 10] {{0x01, 0x02,0x04,0x08,0x10,0x20,0x40,0x80,0x1b,0x36},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00},
                                               {0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}};


              encrypt();
              writeOutput();

          }

          sr.Close();
      }

      static void Decipher()
      {
          //construim pathul de intrare:
          string path = Directory.GetCurrentDirectory();
          path = System.IO.Directory.GetParent(path).FullName;
          path = System.IO.Directory.GetParent(path).FullName;
          path = path + @"\intrare.txt";
          var sr = new StreamReader(path);
          string plainText = sr.ReadLine();
          string key = sr.ReadLine();
          char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

          //facem la fel si cu matricea cheie
          cypherKey = new string[4, 4];
          string[] keyChars = key.Split(delimiterChars);
          string[] chars = plainText.Split(delimiterChars);
          int i = 0, j = 0;
          i = 0;
          j = 0;
          foreach (string s in keyChars)
          {
              cypherKey[j, i] = s;
              j++;
              if (j == 4)
              {
                  j = 0;
                  i++;
              }
          }


          //construim matricea state
          state = new string[4, 4];
          i = 0;
          j = 0;
          int ok = 1;

          foreach (string s in chars)
          {
              if (i != 4)
              {
                  state[j, i] = s;
                  j++;
                  if (j == 4)
                  {
                      j = 0;
                      i++;
                  }
              }
              else
              {
                  decrypt();
                  writeOutput();
                  for (int k = 0; k < 4; k++)
                      for (int l = 0; l < 4; l++)
                          state[k, l] = "00";
                  i = 0;
                  j = 0;
                  ok = 0;
              }

              if (ok == 0)
              {
                  ok = 1;
                  if (i != 4)
                  {
                      state[j, i] = s;
                      j++;
                      if (j == 4)
                      {
                          j = 0;
                          i++;
                      }
                  }
              }
          }
          if (ok == 1)
          {
              decrypt();
              writeOutput();

          }

          sr.Close();
      }



        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            path = System.IO.Directory.GetParent(path).FullName;
            path = System.IO.Directory.GetParent(path).FullName;
            path = path + @"\iesire.txt";
            var sw = new StreamWriter(path);
            sw.Close();

            Cypher();
          //  Decipher();

        }
                   


    }
}
