System.IO.DirectoryInfo di = new DirectoryInfo(BuildStorage);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            DirectoryInfo di1 = new DirectoryInfo(TestStorage);
            foreach (FileInfo file in di1.GetFiles())
            {
                file.Delete();
            }