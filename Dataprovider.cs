using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;

namespace network_toolkit
{
    public class Dataprovider
    {
        private const string database = @"isostore:/network-toolkit.sdf";

        public static void addSpeedTest(double download)
        {
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (!speedTestDataContext.DatabaseExists())
                        speedTestDataContext.CreateDatabase();
                    SpeedTest speedTest = new SpeedTest();
                    speedTest.Created = DateTime.Now;
                    speedTest.Download = download;
                    speedTestDataContext.SpeedTests.InsertOnSubmit(speedTest);
                    speedTestDataContext.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
        }

        //TODO add load more button to history
        public static List<SpeedTest> getSpeedTests(int page = 0)
        {
            List<SpeedTest> speedTests = null;
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (!speedTestDataContext.DatabaseExists())
                        speedTestDataContext.CreateDatabase();
                    speedTests = (from s in speedTestDataContext.SpeedTests
                                  select s).Skip(25 * page).Take(25).ToList();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
            return speedTests;
        }

        //TODO remove
        public static void removeSpeedTest(int id)
        {
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (speedTestDataContext.DatabaseExists())
                    {
                        SpeedTest speedTest = (from s in speedTestDataContext.SpeedTests
                                               where s.Id.Equals(id)
                                               select s) as SpeedTest;
                        speedTestDataContext.SpeedTests.DeleteOnSubmit(speedTest);
                        speedTestDataContext.SubmitChanges();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
        }

        public static void clearDatabase()
        {
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (speedTestDataContext.DatabaseExists())
                        speedTestDataContext.DeleteDatabase();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
        }
    }
}
