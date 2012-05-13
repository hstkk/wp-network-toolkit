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
        private List<double> speeds;

        public Dataprovider(){
            getSpeeds();
        }

        public static void addSpeedTest(SpeedTest speedTest)
        {
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (!speedTestDataContext.DatabaseExists())
                        speedTestDataContext.CreateDatabase();
                    speedTestDataContext.SpeedTests.InsertOnSubmit(speedTest);
                    speedTestDataContext.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
        }

        public static List<SpeedTest> getSpeedTests(/*int page = 0*/)
        {
            List<SpeedTest> speedTests = new List<SpeedTest>();
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (speedTestDataContext.DatabaseExists())
                        speedTests = (from s in speedTestDataContext.SpeedTests
                                      orderby s.Created descending
                                      select s)/*.Skip(25 * page).Take(25)*/.ToList();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
            return speedTests;
        }

        public void getSpeeds()
        {
            speeds = new List<double>();
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (speedTestDataContext.DatabaseExists())
                        speeds = (from s in speedTestDataContext.SpeedTests
                                      orderby s.Created descending
                                      select s.Download).ToList();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
        }

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

        public double average()
        {
            double average = 0.0;
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (speedTestDataContext.DatabaseExists() && speeds != null && speeds.Count > 0)
                    {
                        average = speeds.Average();
                        average = (average > 0) ? average : 0.0;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
            return average;
        }

        public double min()
        {
            double min = 0.0;
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (speedTestDataContext.DatabaseExists() && speeds != null && speeds.Count > 0)
                    {
                        min = speeds.Min();
                        min = (min > 0) ? min : 0.0;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
            return min;
        }

        public double max()
        {
            double max = 0.0;
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (speedTestDataContext.DatabaseExists() && speeds != null && speeds.Count > 0)
                    {
                        max = speeds.Max();
                        max = (max > 0) ? max : 0.0;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
            return max;
        }

        public double last()
        {
            double last = 0.0;
            try
            {
                using (SpeedTestDataContext speedTestDataContext = new SpeedTestDataContext(database))
                {
                    if (speedTestDataContext.DatabaseExists() && speeds != null && speeds.Count > 0)
                    {
                        last = speeds.Last();
                        last = (last > 0) ? last : 0.0;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error can't use database");
            }
            return last;
        }
    }
}
