using System;
using System.Diagnostics;
using System.IO;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

using PdfSharp.Pdf;

using DSManager.Model.Entities;
using DSManager.Model.Enums;
using DSManager.Utilities;

namespace DSManager.PDF.Templates {
    public class PaymentPdf {
        private readonly Payment _payment;

        private Document _document;
        private TextFrame _titleFrame;
        private TextFrame _stampFrame;
        private TextFrame _dateFrame;
        private Table _table;
        private Table _valueToTextTable;
        private Table _footerTable;
        private double separator = 0;

        public PaymentPdf(Payment payment) {
            _payment = payment;
            CreateDocument();
        }

        public void CreateDocument() {
            // TODO przerobić cały dokument, tak aby generował potwierdzenia wpłat
            // Create a new MigraDoc document
            _document = new Document();
            _document.Info.Title = "Dowód wpłaty";
            _document.Info.Subject = "Dokument wygenerowany automatycznie przez aplikację DSManager";
            _document.Info.Author = "DSManager";
            _document.DefaultPageSetup.PageHeight = "12cm";
            _document.DefaultPageSetup.PageWidth = "16cm";
            _document.DefaultPageSetup.LeftMargin = "0.2cm";
            _document.DefaultPageSetup.RightMargin = "0.2cm";
            _document.DefaultPageSetup.TopMargin = "0.2cm";
            _document.DefaultPageSetup.BottomMargin = "0.2cm";

            DefineStyles();
            CreatePage();
            RenderPdf();
        }

        private void DefineStyles() {
            // Get the predefined style Normal.
            Style style = _document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";
            style.Font.Size = 10;

            // Create a new style called Table based on style Normal
            style = _document.Styles.AddStyle("Table", "Normal");
            style.Font.Size = 9;
        }

        private void CreatePage() {
            // Each MigraDoc document needs at least one section.
            Section section = _document.AddSection();

            // Create the text frame for stamp
            _stampFrame = section.AddTextFrame();
            _stampFrame.Height = "1.8cm";
            _stampFrame.Width = "5.5cm";
            _stampFrame.LineFormat.Width = 1;
            _stampFrame.LineFormat.Color = Colors.Black;

            // Put stamp template in frame
            Paragraph paragraph = _stampFrame.AddParagraph("pieczęć");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = 5;
            paragraph.Format.SpaceBefore = "1.55cm";

            // Create the text frame for document title
            _titleFrame = section.AddTextFrame();
            _titleFrame.Height = "1.8cm";
            _titleFrame.Width = "4cm";
            _titleFrame.Left = ShapePosition.Center;
            _titleFrame.RelativeVertical = RelativeVertical.Page;
            _titleFrame.WrapFormat.DistanceTop = "0.2cm";

            // Put title in frame
            paragraph = _titleFrame.AddParagraph("Dowód wpłaty");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = 12;
            paragraph.Format.Font.Bold = true;

            // Put date in frame
            paragraph = _titleFrame.AddParagraph("dnia " + _payment.PaymentDate.ToShortDateString() + " r.");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.SpaceBefore = "0.8cm";

            // Create the text frame for paymentNr
            _dateFrame = section.AddTextFrame();
            _dateFrame.Height = "1.8cm";
            _dateFrame.Width = "5.0cm";
            _dateFrame.Left = ShapePosition.Right;
            _dateFrame.RelativeVertical = RelativeVertical.Page;
            _dateFrame.WrapFormat.DistanceTop = "0.2cm";

            // Put paymentNr in frame
            paragraph = _dateFrame.AddParagraph("Nr " + _payment.PaymentNr);
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.SpaceBefore = "1.3cm";

            paragraph = section.AddParagraph();
            paragraph.Format.SpaceAfter = separator;

            // Create the item table
            _table = section.AddTable();
            _table.Style = "Table";
            _table.Borders.Color = Colors.Black;
            _table.Borders.Width = 0.25;

            // Before you can add a row, you must define the columns
            Column column = _table.AddColumn("9.8cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            Row row = _table.AddRow();
            row.Height = "1cm";
            row.Format.Alignment = ParagraphAlignment.Center;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.Gray;
            row.Cells[0].AddParagraph("Od kogo: " + _payment.Participant.Student.FirstName + " " + _payment.Participant.Student.SecondName + " " + _payment.Participant.Student.LastName);
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].AddParagraph("Winien\nKasa");
            row.Cells[1].MergeRight = 1;
            row.Cells[3].AddParagraph("Ma\nKonto");

            row = _table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Center;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Format.Font.Size = 7;
            row.Shading.Color = Colors.LightGray;
            row.Cells[0].AddParagraph("Za co");
            row.Cells[1].AddParagraph("zł,");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].Borders.Right.Visible = false;
            row.Cells[2].AddParagraph("gr");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[2].Borders.Left.Visible = false;
            row.Cells[3].AddParagraph("numer");

            row = _table.AddRow();
            row.Height = "0.85cm";
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Opłata za kurs prawa jazdy kat. " + _payment.Participant.Course.Category +
                                      " rozpoczętego w dniu " + _payment.Participant.Course.StartDate.ToShortDateString() + " r.");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].AddParagraph(Math.Truncate(_payment.Amount).ToString("#0") + ",");
            row.Cells[1].Borders.Right.Visible = false;
            row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[2].AddParagraph(decimal.Multiply(_payment.Amount - Math.Truncate(_payment.Amount), 100).ToString("00"));
            row.Cells[2].Borders.Left.Visible = false;

            for (var i = 0; i < 4; i++) {
                row = _table.AddRow();
                row.Height = "0.85cm";
                row.Cells[1].Borders.Right.Visible = false;
                row.Cells[2].Borders.Left.Visible = false;
            }

            row = _table.AddRow();
            row.Cells[0].AddParagraph("Razem");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Borders.Visible = false;
            row.Cells[1].AddParagraph(Math.Truncate(_payment.Amount).ToString("#0") + ",");
            row.Cells[1].Borders.Right.Visible = false;
            row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[2].AddParagraph(decimal.Multiply(_payment.Amount - Math.Truncate(_payment.Amount), 100).ToString("00"));
            row.Cells[2].Borders.Left.Visible = false;
            row.Cells[3].Borders.Visible = false;

            paragraph = section.AddParagraph();
            paragraph.Format.SpaceAfter = separator;

            // Create the item table
            _valueToTextTable = section.AddTable();
            _valueToTextTable.Style = "Table";
            _valueToTextTable.Borders.Visible = false;

            // Before you can add a row, you must define the columns
            column = _valueToTextTable.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            _valueToTextTable.AddColumn("14.8cm");

            row = _valueToTextTable.AddRow();
            row.Cells[0].AddParagraph("Słownie złotych");
            row.Cells[0].Format.Font.Size = 6;

            row.Cells[1].AddParagraph(ValueToText.Translate(_payment.Amount, Currency.PLN));
            row.Cells[1].Shading.Color = Colors.LightGray;

            paragraph = section.AddParagraph();
            paragraph.Format.SpaceAfter = separator;

            _footerTable = section.AddTable();
            _footerTable.Style = "Table";
            _footerTable.Format.Font.Size = 8;
            _footerTable.Borders.Width = 0.25;
            _footerTable.Borders.Color = Colors.Black;

            column = _footerTable.AddColumn("2.45cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _footerTable.AddColumn("2.45cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _footerTable.AddColumn("2.45cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _footerTable.AddColumn("2.45cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _footerTable.AddColumn("6cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            row = _footerTable.AddRow();
            row.Height = "1.5cm";

            row.Cells[0].AddParagraph("Wystawił");

            row.Cells[1].AddParagraph("Sprawdził");

            row.Cells[2].AddParagraph("Zatwierdził");

            row.Cells[3].AddParagraph("Rap. kasowy\n\nNr .................\n\npoz. ...........");

            row.Cells[4].AddParagraph("Kwotę powyższą otrzymałem\n\n\n....................................................\npodpis kasjera");
        }

        private void RenderPdf() {
            // Create a renderer
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = _document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            // Save and show the document
            string path = Path.Combine(Directory.GetCurrentDirectory(), "druki", "dowody wpłat");
            string fileName;
            if (string.IsNullOrEmpty(_payment.Participant.Student.SecondName))
                fileName =
                    _payment.Participant.Student.FirstName + " " +
                    _payment.Participant.Student.SecondName + " " +
                    _payment.Participant.Student.LastName + " - " +
                    _payment.PaymentDate.ToShortDateString() + " - " +
                    _payment.PaymentNr + ".pdf";
            else
                fileName =
                    _payment.Participant.Student.FirstName + " " +
                    _payment.Participant.Student.LastName + " - " +
                    _payment.PaymentDate.ToShortDateString() + " - " +
                    _payment.PaymentNr + ".pdf";
            string fullPath = Path.Combine(path, fileName);

            Directory.CreateDirectory(path);
            pdfRenderer.PdfDocument.Save(fullPath);
            Process.Start(fullPath);
        }
    }
}