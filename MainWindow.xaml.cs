using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Paragraph = iTextSharp.text.Paragraph;
using Rectangle = iTextSharp.text.Rectangle;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static double Balance = 0;
        private static string previousText = "";

        public static double prevBalance = 0;
        
        public MainWindow()
        {
            InitializeComponent();
            
            previousText = rate.Text;





            //  DataGridXAML.ItemsSource = DataGridXAML.ItemsSource;
        }
        //  public DataGridXAML;
        public class Data
        {
            public string RefNo { get; set; }
            public string Narration { get; set; }
           // public string Product { get; set; }
         //   public string Items { get; set; }
            public string Kuantity { get; set; }
            public string Rate { get; set; }
            public string Amount { get; set; }
            public string Debit { get; set; }
            public string Credit { get; set; }
            public string Balance { get; set; }

            public string Date { get; set; }




        }
        static void createXmlFile()
        {

            XmlDocument docx = new XmlDocument();
            XmlElement root = docx.CreateElement("Data");
            docx.AppendChild(root);
            docx.Save(@"smash.xml");


        }
        static void ReadXml()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(@"smash.xml");
            XmlNodeList nodes = xdoc.SelectNodes("Data/Entry");
            foreach (XmlNode node in nodes)
            {

                XmlNode bal = node.SelectSingleNode("Balance");
                Balance = Convert.ToDouble(bal.InnerText);
            }

        }



        private async void save_Click(object sender, RoutedEventArgs e)
        {
            // || balance.Text=="" || balance.Text==" "
            if (refno.Text == "" || refno.Text == " " )
            {
                MessageBox.Show("Title cannot be empty \n Balance cannot be empty", "Title", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
            else
            {

               
                if (rate.Text == "")
                {
                    rate.Text = "0";
                                    }
                if (credit.Text == "")
                {
                    credit.Text = "0";
                }
                if (debit.Text == "")
                {
                    debit.Text = "0";
                }
                if (kuantity.Text == "")
                {
                    kuantity.Text = "0";
                   
                }

                


                string amount = "0";

                 Double k = Convert.ToDouble(kuantity.Text);
                 Double r = Convert.ToDouble(rate.Text);
                 Double ans = k * r;
                 amount = Convert.ToString(ans);



                prevBalance = 0;
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(@"smash.xml");
                XmlNodeList nodes = xdoc.SelectNodes("Data/Entry");
                bool c = true;
                bool warn = true;
                foreach (XmlNode node in nodes)
                {
                    XmlNode refn = node.SelectSingleNode("RefNo");
                    if (refn.InnerText == refno.Text)
                    {
                        XmlNode bal = node.SelectSingleNode("Balance");
                        if (bal.InnerText=="") { bal.InnerText = "0"; }
                        prevBalance = Convert.ToDouble(bal.InnerText);
                    }

                }
                








                XmlDocument docx = new XmlDocument();

                docx.Load(@"smash.xml");
                XmlNode root = docx.SelectSingleNode("Data");
                XmlElement movies = docx.CreateElement("Entry");
                root.AppendChild(movies);

                XmlAttribute id = docx.CreateAttribute("id");
                id.Value = docx.SelectNodes("Data/Entry").Count.ToString();
                movies.Attributes.Append(id);
                XmlElement Refno = docx.CreateElement("RefNo");
                Refno.InnerText = refno.Text;
              
                movies.AppendChild(Refno);
                XmlElement narrat = docx.CreateElement("Narration");
                narrat.InnerText = narration.Text;
                movies.AppendChild(narrat);
               
                XmlElement kuan = docx.CreateElement("Quantity");
                if (kuantity.Text == "0")
                {
                    kuan.InnerText = "";
                }
                else
                {
                    kuan.InnerText = kuantity.Text;
                }

                movies.AppendChild(kuan);
                XmlElement rat = docx.CreateElement("Rate");
                if (rate.Text == "0")
                {
                    rat.InnerText = "";
                }
                else
                {
                    rat.InnerText = rate.Text;
                }

                movies.AppendChild(rat);
                XmlElement amt = docx.CreateElement("Amount");
                if (amount == "0")
                {
                    amt.InnerText = "";
                }
                else
                {
                    amt.InnerText = amount;
                }

                movies.AppendChild(amt);
                XmlElement deb = docx.CreateElement("Debit");
                if (debit.Text == "0")
                {
                    deb.InnerText = "";
                }
                else
                {
                    deb.InnerText = debit.Text;
                }

                movies.AppendChild(deb);
               double dobDebit = Convert.ToDouble(debit.Text);
                double dobCredit = Convert.ToDouble(credit.Text);
                double dobBalance = dobDebit - dobCredit;
                double dobBala = prevBalance + dobBalance;
                Balance = dobBala;
                String Balce = Convert.ToString(dobBala);
                XmlElement cred = docx.CreateElement("Credit");
                if (credit.Text == "0")
                {
                    cred.InnerText = "";
                }
                else
                {
                    cred.InnerText = credit.Text;
                }

                movies.AppendChild(cred);
                XmlElement balan = docx.CreateElement("Balance");
                balan.InnerText = Balce;
                if (balan.InnerText == "0")
                {
                    balan.InnerText = "";
                }




                movies.AppendChild(balan);

                XmlElement dat = docx.CreateElement("Date");
                dat.InnerText = DateTime.Now.ToShortDateString();
                movies.AppendChild(dat);

                docx.Save(@"smash.xml");



                Data NewData = new Data();
                NewData.RefNo = Refno.InnerText;
                NewData.Narration = narrat.InnerText;
               // NewData.Product = produc.InnerText;
               // NewData.Items = itm.InnerText;
                NewData.Kuantity = kuan.InnerText;
                NewData.Rate = rat.InnerText;
                NewData.Amount = amt.InnerText;
                NewData.Debit = deb.InnerText;
                NewData.Credit = cred.InnerText;
                NewData.Balance = balan.InnerText;
                NewData.Date = dat.InnerText;




                DataGridXAML.Items.Add(NewData);

                refno.Text = "";
                narration.Text = "";
                kuantity.Text = "";
                debit.Text = "";
                credit.Text = "";
                balance.Text = "";
                rate.Text = "";

                sucess.Visibility = Visibility.Visible;
                await Task.Delay(1500);
                sucess.Visibility = Visibility.Hidden;
            }



        }

        private void pdf_Click(object sender, RoutedEventArgs e)
        {
            string folderName = @"C:\BPC";

            System.IO.Directory.CreateDirectory(folderName);

            // Create a file name for the file you want to create.
            string fileName = String.Format("Report {0}.pdf",
                                DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));

            folderName = System.IO.Path.Combine(folderName, fileName);




            //left right top bottom   DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss"));
            Document document = new Document(PageSize.A4.Rotate(), 20, 20, 50, 20);
            string filename = String.Format("Report {0}.pdf",
                                DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(folderName, FileMode.Create));
            writer.PageEvent = new HeaderFooter();

            document.Open();

            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.NORMAL);


            PdfPTable table = new PdfPTable(11);
            table.WidthPercentage = 100;
            table.SpacingBefore = 10;
            table.DefaultCell.Border = Rectangle.BOX;


            PdfPCell cell10 = new PdfPCell(new Phrase("Date", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            cell10.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell10.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell10);


            PdfPCell cell = new PdfPCell(new Phrase("Title", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Colspan = 2;
            cell.Border = Rectangle.BOX;
            table.AddCell(cell);
            table.HeaderRows = 1;
            PdfPCell cell1 = new PdfPCell(new Phrase("Details", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;

            cell1.Colspan = 2;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell1);
     
            PdfPCell cell4 = new PdfPCell(new Phrase("Quantity", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            cell5.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell5);
            PdfPCell cell6 = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            cell6.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell6.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell6);
            PdfPCell cell7 = new PdfPCell(new Phrase("Debit", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            cell7.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell7.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell7);
            PdfPCell cell8 = new PdfPCell(new Phrase("Credit", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            cell8.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell8.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell8);
            PdfPCell cell9 = new PdfPCell(new Phrase("Balance", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            cell9.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell9.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell9);




            string hello = "";
            string datee = "";

            double Totalkuantity = 0;
            double TotalDebit = 0;
            double TotalCredit = 0;
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(@"smash.xml");
            XmlNodeList nodes = xdoc.SelectNodes("Data/Entry");
            bool check = true;
            var PrevDaTe = "";
            foreach (XmlNode node in nodes)
            {
               
                if ((node.Attributes[0].Value) == "1")
                {

                    XmlNode dt = node.SelectSingleNode("Date");
                    hello = dt.InnerText;
                    PrevDaTe = dt.InnerText;
                }

                XmlNode dte = node.SelectSingleNode("Date");
                if (PrevDaTe == dte.InnerText)
                {
                    check = false;
                }
                else
                {
                    check = true;
                }
                PdfPCell dtc = new PdfPCell(new Phrase(dte.InnerText));
                if (check == false)
                {

                    dtc.DisableBorderSide(Rectangle.TOP_BORDER);
                }
                dtc.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table.AddCell(dtc);

                

                XmlNode refn = node.SelectSingleNode("RefNo");
                PdfPCell cell01 = new PdfPCell(new Phrase(refn.InnerText));
                cell01.HorizontalAlignment = Element.ALIGN_CENTER;
                cell01.Border = Rectangle.BOX;
                cell01.Colspan = 2;
                if (check == false)
                {

                    cell01.DisableBorderSide(Rectangle.TOP_BORDER);
                }
                cell01.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table.AddCell(cell01);
                XmlNode nar = node.SelectSingleNode("Narration");
                PdfPCell ce = new PdfPCell(new Phrase(nar.InnerText));
                ce.Colspan = 2;
                if (check == false)
                {

                    ce.DisableBorderSide(Rectangle.TOP_BORDER);
                }
                ce.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table.AddCell(ce);
               
                XmlNode kuan = node.SelectSingleNode("Quantity");
                PdfPCell kuanc = new PdfPCell(new Phrase(kuan.InnerText));
                if (kuan.InnerText == "") { kuan.InnerText = "0"; }
                if (check == false)
                {

                    kuanc.DisableBorderSide(Rectangle.TOP_BORDER);
                }
                kuanc.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table.AddCell(kuanc);

                Totalkuantity = Totalkuantity + Convert.ToDouble(kuan.InnerText);
                XmlNode rat = node.SelectSingleNode("Rate");
                PdfPCell ratc = new PdfPCell(new Phrase(rat.InnerText));
                if (rat.InnerText == "") { rat.InnerText = "0"; }
                if (check == false)
                {

                    ratc.DisableBorderSide(Rectangle.TOP_BORDER);
                }
                ratc.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table.AddCell(ratc);

                XmlNode amt = node.SelectSingleNode("Amount");
                PdfPCell amtc = new PdfPCell(new Phrase(amt.InnerText));
                if (amt.InnerText == "") { amt.InnerText = "0"; }

                if (check == false)
                {

                    amtc.DisableBorderSide(Rectangle.TOP_BORDER);
                }
                amtc.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table.AddCell(amtc);

                XmlNode de = node.SelectSingleNode("Debit");
                PdfPCell dec = new PdfPCell(new Phrase(de.InnerText));
                if (de.InnerText == "") { de.InnerText = "0"; }

                if (check == false)
                {

                    dec.DisableBorderSide(Rectangle.TOP_BORDER);
                }
                dec.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table.AddCell(dec);

                TotalDebit = TotalDebit + Convert.ToDouble(de.InnerText);
                XmlNode cre = node.SelectSingleNode("Credit");
                PdfPCell crec = new PdfPCell(new Phrase(cre.InnerText));
                if (cre.InnerText == "") { cre.InnerText = "0"; }

                if (check == false)
                {

                    crec.DisableBorderSide(Rectangle.TOP_BORDER);
                }
                crec.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table.AddCell(crec);

                TotalCredit = TotalCredit + Convert.ToDouble(cre.InnerText);
                XmlNode bal = node.SelectSingleNode("Balance");
                if (bal.InnerText == "") { bal.InnerText = "0"; }
                if (Convert.ToDouble(bal.InnerText) < 0)
                {
                    PdfPCell balcel = new PdfPCell(new Phrase(bal.InnerText, FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.RED)));
                    if (check == false)
                    {

                        balcel.DisableBorderSide(Rectangle.TOP_BORDER);
                    }
                    balcel.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                    table.AddCell(balcel);
                }
                else
                {
                    PdfPCell balcel = new PdfPCell(new Phrase(bal.InnerText));
                    if (check == false)
                    {

                        balcel.DisableBorderSide(Rectangle.TOP_BORDER);
                    }
                    balcel.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                    table.AddCell(balcel);
                }



                PrevDaTe = dte.InnerText;

                datee = dte.InnerText;
            }
            //  Paragraph paragr = new Paragraph(" \n" + "\n" + "\n");
            // document.Add(paragr);
            Paragraph paragraph = new Paragraph(" From: " + hello + "  To: " + datee);


            document.Add(paragraph);


            PdfPCell TB = new PdfPCell(new Phrase("Grand Total: ", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            TB.HorizontalAlignment = 1;
            TB.Colspan = 5;
            table.AddCell(TB);

            PdfPCell BT = new PdfPCell(new Phrase(Convert.ToString(Totalkuantity), FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            BT.Colspan = 1;
            BT.HorizontalAlignment = 1;
            table.AddCell(BT);
            PdfPCell awien = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            awien.Colspan = 1;
            awien.HorizontalAlignment = 1;
            table.AddCell(awien);

            table.AddCell(awien);
            PdfPCell det = new PdfPCell(new Phrase(Convert.ToString(TotalDebit), FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            det.Colspan = 1;
            det.HorizontalAlignment = 1;
            table.AddCell(det);
            PdfPCell cred = new PdfPCell(new Phrase(Convert.ToString(TotalCredit), FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
            cred.Colspan = 1;
            cred.HorizontalAlignment = 1;
            table.AddCell(cred);
            table.AddCell(awien);

            table.SpacingAfter = 5;


            document.Add(table);




            document.Close();
            MessageBox.Show("PDF Generated", "PDF", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        class HeaderFooter : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {

                Paragraph paragr = new Paragraph(" \n" + "\n" + "\n" + "\n");
                document.Add(paragr);

            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {

                PdfPTable tbheader = new PdfPTable(1);
                tbheader.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                tbheader.DefaultCell.Border = 0;
                tbheader.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell cell01 = new PdfPCell(new Paragraph("Party Ledger Report", FontFactory.GetFont(FontFactory.TIMES_BOLD, 18, Font.UNDERLINE, BaseColor.BLUE)));
                // cell01.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell01.HorizontalAlignment = Element.ALIGN_CENTER;
                cell01.Border = 0;
                tbheader.AddCell(cell01);
                Paragraph paragraph = new Paragraph("\n\nBILAL AND BROTHERS PET PLASTIC COMPLEX");


                tbheader.AddCell(paragraph);

                tbheader.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin), writer.DirectContent);

                PdfPTable tbfooter = new PdfPTable(2);
                tbfooter.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                tbfooter.DefaultCell.Border = 0;
                LineSeparator line = new LineSeparator(0.5f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
                document.Add(line);
                var _cell3 = new PdfPCell(new Paragraph("Generated at: " + DateTime.Now.ToString()));
                _cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                _cell3.Border = 0;
                tbfooter.AddCell(_cell3);

                var _celly = new PdfPCell(new Paragraph(writer.PageNumber.ToString()));//For page no.
                _celly.HorizontalAlignment = Element.ALIGN_RIGHT;
                _celly.Border = 0;
                tbfooter.AddCell(_celly);
                float[] widths1 = new float[] { 20f, 20f };
                tbfooter.SetWidths(widths1);
                tbfooter.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin), writer.DirectContent);

            }
        }
        private void rate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
                previousText = "";
            else
            {
               // (success )
                double num = 0;
                bool success = double.TryParse(((TextBox)sender).Text, out num);
                if (success & num >= 0 )
                {
                    ((TextBox)sender).Text.Trim();
                    previousText = ((TextBox)sender).Text;
                }
                else
                {
                    ((TextBox)sender).Text = previousText;
                    ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                }
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {

            refno.Text = "";
            narration.Text = "";
           
            kuantity.Text = "";
            debit.Text = "";
            credit.Text = "";
            balance.Text = "";
            rate.Text = "";

        }

        private void new_Click(object sender, RoutedEventArgs e)
        {

            var reslt = MessageBox.Show("This will erase ALL Data\n" + "Press OK to Delete Data \n" + "Press Cancel to cancel", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (reslt == MessageBoxResult.OK)
            {
                createXmlFile();
                

            }

        }
      
        private void search_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(@"smash.xml");
            XmlNodeList nodes = xdoc.SelectNodes("Data/Entry");
            bool c = true;
            bool warn = true;
            foreach (XmlNode node in nodes)
            {
                XmlNode refn = node.SelectSingleNode("RefNo");
                if((refn.InnerText==find.Text) && c==true)
                {
                    warn = false;
                    c = false;
                    string folderName = @"C:\BPC";


                    System.IO.Directory.CreateDirectory(folderName);

                    // Create a file name for the file you want to create.
                    string fileName = String.Format("Customer {0}.pdf",
                                        DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));

                    folderName = System.IO.Path.Combine(folderName, fileName);



                    //left right top bottom   DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss"));
                    Document document = new Document(PageSize.A4.Rotate(), 20, 20, 50, 20);
                  //  string filename = String.Format("Customer {0}.pdf",
                           //             DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(folderName, FileMode.Create));
                    writer.PageEvent = new HeaderFooter();

                    document.Open();

                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.NORMAL);




                    PdfPTable table = new PdfPTable(11);
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 10;
                    table.DefaultCell.Border = Rectangle.BOX;


                    PdfPCell cell10 = new PdfPCell(new Phrase("Date", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell10.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell10.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell10);


                    PdfPCell cell = new PdfPCell(new Phrase("Title", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Colspan = 2;
                    cell.Border = Rectangle.BOX;
                    table.AddCell(cell);
                    table.HeaderRows = 1;
                    PdfPCell cell1 = new PdfPCell(new Phrase("Details", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell1.BackgroundColor = BaseColor.LIGHT_GRAY;

                    cell1.Colspan = 2;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell1);
                    /*
                    PdfPCell cell2 = new PdfPCell(new Phrase("Product", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell2);
                    */
                    /*
                    PdfPCell cell3 = new PdfPCell(new Phrase("Items", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                    */
                    //  table.AddCell(cell3);
                    PdfPCell cell4 = new PdfPCell(new Phrase("Quantity", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell4);
                    PdfPCell cell5 = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell5.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell5);
                    PdfPCell cell6 = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell6.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell6);
                    PdfPCell cell7 = new PdfPCell(new Phrase("Debit", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell7.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell7.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell7);
                    PdfPCell cell8 = new PdfPCell(new Phrase("Credit", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell8.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell8.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell8);
                    PdfPCell cell9 = new PdfPCell(new Phrase("Balance", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cell9.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell9.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell9);




                    string hello = "";
                    string datee = "";

                    double Totalkuantity = 0;
                    double TotalDebit = 0;
                    double TotalCredit = 0;
                    XmlDocument docx = new XmlDocument();
                    xdoc.Load(@"smash.xml");
                    XmlNodeList nodess = xdoc.SelectNodes("Data/Entry");
                    bool check = true;
                    var PrevDaTe = "";
                    bool gh = true;
                    bool fistDate = true;
                    foreach (XmlNode hulk in nodess)
                    {
                        XmlNode refnnn = hulk.SelectSingleNode("RefNo");
                        if (refnnn.InnerText==find.Text && gh==true)
                        {
                           // gh= false;
                            if ((hulk.Attributes[0].Value) == "1")
                            {

                                XmlNode dt = hulk.SelectSingleNode("Date");
                                hello = dt.InnerText;
                                PrevDaTe = dt.InnerText;
                            }

                            XmlNode dte = hulk.SelectSingleNode("Date");
                            
                            if (PrevDaTe == dte.InnerText)
                            {
                                check = false;
                            }
                            else
                            {
                                check = true;
                            }
                            PdfPCell dtc = new PdfPCell(new Phrase(dte.InnerText));
                            if(fistDate==true)
                            {
                                fistDate = false;
                                hello=dte.InnerText;

                            }
                            if (check == false)
                            {

                                dtc.DisableBorderSide(Rectangle.TOP_BORDER);
                            }
                            dtc.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                            table.AddCell(dtc);

                            if (PrevDaTe == dte.InnerText)
                            {
                                check = false;
                            }
                            else
                            {
                                check = true;
                            }

                            XmlNode refnn = hulk.SelectSingleNode("RefNo");
                            PdfPCell cell01 = new PdfPCell(new Phrase(refnn.InnerText));
                            cell01.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell01.Border = Rectangle.BOX;
                            cell01.Colspan = 2;
                            if (check == false)
                            {

                                cell01.DisableBorderSide(Rectangle.TOP_BORDER);
                            }
                            cell01.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                            table.AddCell(cell01);
                            XmlNode nar = hulk.SelectSingleNode("Narration");
                            PdfPCell ce = new PdfPCell(new Phrase(nar.InnerText));
                            ce.Colspan = 2;
                            if (check == false)
                            {

                                ce.DisableBorderSide(Rectangle.TOP_BORDER);
                            }
                            ce.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                            table.AddCell(ce);
                       /*
                            XmlNode pro = hulk.SelectSingleNode("Product");
                            PdfPCell prc = new PdfPCell(new Phrase(pro.InnerText));

                            if (check == false)
                            {

                                prc.DisableBorderSide(Rectangle.TOP_BORDER);
                            }
                            prc.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                            table.AddCell(prc);
                       */
                            /* 
                               XmlNode itme = node.SelectSingleNode("Items");
                               PdfPCell itc = new PdfPCell(new Phrase(itme.InnerText));

                               if (check == false)
                               {

                                   itc.DisableBorderSide(Rectangle.TOP_BORDER);
                               }
                               itc.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                               table.AddCell(itc);
                             */
                            XmlNode kuan = hulk.SelectSingleNode("Quantity");
                            PdfPCell kuanc = new PdfPCell(new Phrase(kuan.InnerText));
                            if (kuan.InnerText == "") { kuan.InnerText = "0"; }
                            if (check == false)
                            {

                                kuanc.DisableBorderSide(Rectangle.TOP_BORDER);
                            }
                            kuanc.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                            table.AddCell(kuanc);

                            Totalkuantity = Totalkuantity + Convert.ToDouble(kuan.InnerText);
                            XmlNode rat = hulk.SelectSingleNode("Rate");
                            PdfPCell ratc = new PdfPCell(new Phrase(rat.InnerText));
                            if (rat.InnerText == "") { rat.InnerText = "0"; }
                            if (check == false)
                            {

                                ratc.DisableBorderSide(Rectangle.TOP_BORDER);
                            }
                            ratc.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                            table.AddCell(ratc);

                            XmlNode amt = hulk.SelectSingleNode("Amount");
                            PdfPCell amtc = new PdfPCell(new Phrase(amt.InnerText));
                            if (amt.InnerText == "") { amt.InnerText = "0"; }

                            if (check == false)
                            {

                                amtc.DisableBorderSide(Rectangle.TOP_BORDER);
                            }
                            amtc.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                            table.AddCell(amtc);

                            XmlNode de = hulk.SelectSingleNode("Debit");
                            PdfPCell dec = new PdfPCell(new Phrase(de.InnerText));
                            if (de.InnerText == "") { de.InnerText = "0"; }

                            if (check == false)
                            {

                                dec.DisableBorderSide(Rectangle.TOP_BORDER);
                            }
                            dec.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                            table.AddCell(dec);

                            TotalDebit = TotalDebit + Convert.ToDouble(de.InnerText);
                            XmlNode cre = hulk.SelectSingleNode("Credit");
                            PdfPCell crec = new PdfPCell(new Phrase(cre.InnerText));
                            if (cre.InnerText == "") { cre.InnerText = "0"; }

                            if (check == false)
                            {

                                crec.DisableBorderSide(Rectangle.TOP_BORDER);
                            }
                            crec.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                            table.AddCell(crec);

                            TotalCredit = TotalCredit + Convert.ToDouble(cre.InnerText);
                            XmlNode bal = hulk.SelectSingleNode("Balance");
                            if (bal.InnerText == "") { bal.InnerText = "0"; }
                            if (Convert.ToDouble(bal.InnerText) < 0)
                            {
                                PdfPCell balcel = new PdfPCell(new Phrase(bal.InnerText, FontFactory.GetFont(FontFactory.HELVETICA,12,BaseColor.RED)));
                                if (check == false)
                                {

                                    balcel.DisableBorderSide(Rectangle.TOP_BORDER);
                                }
                                balcel.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                                table.AddCell(balcel);
                            }
                            else
                            {
                                PdfPCell balcel = new PdfPCell(new Phrase(bal.InnerText));
                                if (check == false)
                                {

                                    balcel.DisableBorderSide(Rectangle.TOP_BORDER);
                                }
                                balcel.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                                table.AddCell(balcel);
                            }



                            PrevDaTe = dte.InnerText;

                            datee = dte.InnerText;






                        }
                        
                       
                    }
                    //  Paragraph paragr = new Paragraph(" \n" + "\n" + "\n");
                    // document.Add(paragr);
                    Paragraph paragraph = new Paragraph(" From: " + hello + "  To: " + datee);


                    document.Add(paragraph);


                    PdfPCell TB = new PdfPCell(new Phrase("Grand Total: ", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    TB.HorizontalAlignment = 1;
                    TB.Colspan = 5;
                    table.AddCell(TB);

                    PdfPCell BT = new PdfPCell(new Phrase(Convert.ToString(Totalkuantity), FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    BT.Colspan = 1;
                    BT.HorizontalAlignment = 1;
                    table.AddCell(BT);
                    PdfPCell awien = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    awien.Colspan = 1;
                    awien.HorizontalAlignment = 1;
                    table.AddCell(awien);

                    table.AddCell(awien);
                    PdfPCell det = new PdfPCell(new Phrase(Convert.ToString(TotalDebit), FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    det.Colspan = 1;
                    det.HorizontalAlignment = 1;
                    table.AddCell(det);
                    PdfPCell cred = new PdfPCell(new Phrase(Convert.ToString(TotalCredit), FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, BaseColor.BLUE)));
                    cred.Colspan = 1;
                    cred.HorizontalAlignment = 1;
                    table.AddCell(cred);
                    table.AddCell(awien);

                    table.SpacingAfter = 5;


                    document.Add(table);




                    document.Close();
                    MessageBox.Show("PDF Generated", "PDF", MessageBoxButton.OK, MessageBoxImage.Information);
 
                }
                if(warn==false)
                {
                    break;
                }

            }
            if(warn==true)
            {
                MessageBox.Show("No entry found.Enter exact same title", "Not Found", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
               
        }

        private void hello(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            
            
                e.Handled = new Regex("[^0-9-]+").IsMatch(e.Text);
             
        }

        private void Onlydigit(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

    
    
    }
}
