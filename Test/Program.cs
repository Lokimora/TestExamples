using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security.Policy;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Xml;
using Iveonik.Stemmers;
using MongoDB.Bson;
using SevenZip;
using CompressionMode = System.IO.Compression.CompressionMode;
using Encoder = System.Text.Encoder;

namespace Test
{
    internal class Program
    {

        private static void Main(string[] args)
        {

            string baseAddress = "ftp://127.0.0.1:14147/240x400_targetix";

            FtpClient client = new FtpClient(baseAddress, "a.kozlov", "");

            client.GetDirectoryInformation(baseAddress);

        }

        /// <summary>
        /// Проврека выставления общего бюджета для таргета
        /// </summary>
        /// <param name="balance">Текщуий баланс рекламодателя</param>
        /// <param name="totalAmount">Выставленный общий бюджет для рК</param>
        /// <param name="spend">Денег уже потрачено в статистике</param>
        /// <returns></returns>
        private static string GetBudget(decimal balance, float? totalAmount, decimal spend)
        {

            var percentNds = 20;


            var totalSpend = spend;


            if (totalSpend != 0)
            {
                totalSpend = (totalSpend % 100 == 0)
                    ? (int)totalSpend
                    : (int)Math.Floor(totalSpend / 100) * 100;
            }


            if (totalAmount != null)
            {
                var budget = (float)balance < totalAmount ? (float)balance : totalAmount.Value;

                if (budget < 600)
                {
                    budget = 600 + (int)totalSpend;
                    return budget.ToString();
                }

                var budgetWithoutComission = budget / (100 + percentNds) * 100;

                var budgetStep = (budgetWithoutComission % 100 == 0)
                    ? budgetWithoutComission
                    : Math.Floor(budgetWithoutComission / 100d) * 100;

                var budgFloor = (int)budgetStep + totalSpend;

                return budgFloor.ToString();
            }


            if (balance < 600)
            {
                var res = 600 + totalSpend;

                return res.ToString();
            }

            else
            {
                var budgetWithoutComission = balance / (100 + percentNds) * 100;

                var budgetStep = (budgetWithoutComission % 100 == 0)
                    ? budgetWithoutComission
                    : Math.Floor(budgetWithoutComission / 100) * 100;

                var budgFloor = (int)budgetStep + totalSpend;

                return budgFloor.ToString();
            }



        }
    }

}
