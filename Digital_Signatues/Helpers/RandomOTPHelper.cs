﻿using System;

namespace Digital_Signatues.Helpers
{
    public class RandomOTPHelper
    {
        public static string random()
        {
			var chars1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
			var stringChars1 = new char[6];
			var random1 = new Random();

			for (int i = 0; i < stringChars1.Length; i++)
			{
				stringChars1[i] = chars1[random1.Next(chars1.Length)];
			}

			var str = new String(stringChars1);
			return str;
		}
		public static string randomQR()
		{
			var chars1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			var stringChars1 = new char[6];
			var random1 = new Random();

			for (int i = 0; i < stringChars1.Length; i++)
			{
				stringChars1[i] = chars1[random1.Next(chars1.Length)];
			}

			var str = new String(stringChars1);
			return str;
		}
	}
}
