using System;
using System.Collections;

namespace HammingCoderVol2
{
    class Program
    {
        static BitArray Code(string inMessage)
        {
            var messageArray = new BitArray(inMessage.Length, false);
            for (int i = 0; i < inMessage.Length; i++)
            {
                if (inMessage[i] == '1')
                    messageArray[i] = true;
                else
                    messageArray[i] = false;
            }
            int messageInd = 0;
            int retInd = 0;
            int controlIndex = 1;
            var retArray = new BitArray(messageArray.Length + 1 + (int)Math.Ceiling(Math.Log(messageArray.Length, 2)));
            while (messageInd < messageArray.Length)
            {
                if (retInd + 1 == controlIndex)
                {
                    retInd++;
                    controlIndex = controlIndex * 2;
                    continue;
                }
                retArray.Set(retInd, messageArray.Get(messageInd));
                messageInd++;
                retInd++;
            }
            retInd = 0;
            controlIndex = 1 << (int)Math.Log(retArray.Length, 2);
            while (controlIndex > 0)
            {
                int c = controlIndex - 1;
                int counter = 0;

                while (c < retArray.Length)
                {
                    for (int i = 0; i < controlIndex && c < retArray.Length; i++)
                    {
                        if (retArray.Get(c))
                            counter++;
                        c++;
                    }
                    c += controlIndex;
                }

                if (counter % 2 != 0) retArray.Set(controlIndex - 1, true);
                controlIndex = controlIndex / 2;
            }
            return retArray;
        }

        static BitArray Decode(string inMessage)
        {
            var codedArray = new BitArray(inMessage.Length, false);
            for(int i = 0; i < codedArray.Length; i++)
            {
                if (inMessage[i] == '1')
                    codedArray[i] = true;
                else
                    codedArray[i] = false;
            }
            var decodedArray = new BitArray((int)(codedArray.Count - Math.Ceiling(Math.Log(codedArray.Count, 2))), false);
            int count = 0;
            for (int i = 0; i < codedArray.Length; i++)
            {
                for (int j = 0; j < Math.Ceiling(Math.Log(codedArray.Count, 2)); j++)
                {
                    if (i == Math.Pow(2, j) - 1)
                        i++;
                }
                decodedArray[count] = codedArray[i];
                count++;
            }
            string strDecodedArray = "";
            for (int i = 0; i < decodedArray.Length; i++)
            {
                if (decodedArray[i])
                    strDecodedArray += "1";
                else
                    strDecodedArray += "0";
            }
            var checkArray = Code(strDecodedArray);
            byte[] failBits = new byte[checkArray.Length - decodedArray.Length];
            count = 0;
            bool isMistake = false;
            for(int i = 0; i < checkArray.Length - decodedArray.Length; i++)
            {
                if (codedArray[(int)Math.Pow(2, i) - 1] != checkArray[(int)Math.Pow(2, i) - 1])
                {
                    failBits[count] = (byte)(Math.Pow(2, i));
                    count++;
                    isMistake = true;
                }
            }
            if (isMistake)
            {
                int mistakeIndex = 0;
                for (int i = 0; i < failBits.Length; i++)
                    mistakeIndex += failBits[i];
                mistakeIndex--;
                codedArray.Set(mistakeIndex, !codedArray[mistakeIndex]);
                Console.WriteLine($"Ошибка в бите №{mistakeIndex}");
                count = 0;
                for (int i = 0; i < codedArray.Length; i++)
                {
                    for (int j = 0; j < Math.Ceiling(Math.Log(codedArray.Count, 2)); j++)
                    {
                        if (i == Math.Pow(2, j) - 1)
                            i++;
                    }
                    decodedArray[count] = codedArray[i];
                    count++;
                }
            }
            return decodedArray;
        }

        static void Main(string[] args)
        {
            string choice = null;
            BitArray code;
            while (choice != "3")
            {
                Console.Write("1. Закодировать сообщение\n2. Декодировать сообщение\n3. Выйти из программы\n");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Введите сообщение: ");
                        code = Code(Console.ReadLine());
                        for (int i = 0; i < code.Length; i++)
                            Console.Write(code[i] ? "1" : "0");
                        break;
                    case "2":
                        Console.Write("Введите сообщение: ");
                        code = Decode(Console.ReadLine());
                        for (int i = 0; i < code.Length; i++)
                            Console.Write(code[i] ? "1" : "0");
                        break;
                    case "3":
                        break;
                    default:
                        Console.WriteLine("Неверное число");
                        break;
                }
                Console.WriteLine();
            }
        }
    }
}