using System.Diagnostics;
using System.IO;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

using PdfSharp.Pdf;

using DSManager.Model.Entities;

namespace DSManager.PDF.Templates {
    public class PaymentPdf {
        private readonly Payment _payment;

        private Document _document;
        private TextFrame _titleFrame;
        private TextFrame _stampFrame;
        private TextFrame _dateFrame;
        private Table _personTable;
        private Table _personTable2;
        private Table _table;

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
            _document.DefaultPageSetup.LeftMargin = 25;
            _document.DefaultPageSetup.RightMargin = 25;
            _document.DefaultPageSetup.TopMargin = 35;
            _document.DefaultPageSetup.BottomMargin = 120;
            _document.DefaultPageSetup.DifferentFirstPageHeaderFooter = true;

            DefineStyles();
            CreatePage();
            FillContent();
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
            _stampFrame.Height = "2.5cm";
            _stampFrame.Width = "7cm";
            _stampFrame.RelativeVertical = RelativeVertical.Page;
            _stampFrame.RelativeHorizontal = RelativeHorizontal.Page;
            _stampFrame.WrapFormat.DistanceTop = "1cm";
            _stampFrame.WrapFormat.DistanceLeft = "0.5cm";

            // Put stamp template in frame
            Paragraph paragraph = _stampFrame.AddParagraph(
                "............................................................................."
                + "\n" + "(nazwa ośrodka szkolenia kierowców wydającego kartę)");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = 6.5;
            paragraph.Format.SpaceBefore = "2cm";

            // Create the text frame for document title
            _titleFrame = section.AddTextFrame();
            _titleFrame.Height = "2.5cm";
            _titleFrame.Width = "6.0cm";
            _titleFrame.Left = ShapePosition.Center;
            _titleFrame.RelativeVertical = RelativeVertical.Page;
            _titleFrame.WrapFormat.DistanceTop = "1cm";
            _titleFrame.LineFormat.Width = 1;
            _titleFrame.LineFormat.Color = Colors.Black;

            // Put title in frame
            paragraph = _titleFrame.AddParagraph("Karta przeprowadzonych zajęć");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = 11;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.SpaceBefore = "0.75cm";

            // Create the text frame for document date
            _dateFrame = section.AddTextFrame();
            _dateFrame.Height = "2.5cm";
            _dateFrame.Width = "6.0cm";
            _dateFrame.Left = ShapePosition.Right;
            _dateFrame.RelativeVertical = RelativeVertical.Page;
            _dateFrame.WrapFormat.DistanceTop = "1cm";

            // Put date in frame
            paragraph = _dateFrame.AddParagraph("Data wydania ");
            paragraph.AddDateField("dd.MM.yyyy");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Right;

            // Add student info section
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "2.5cm";
            paragraph.Format.SpaceAfter = "0.5cm";
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddText("w zakresie prawa jazdy kategorii " + _payment.Amount + " na kursie dla kandydatów na kierowców lub motorniczych");

            _personTable = section.AddTable();
            _personTable.Style = "Table";
            _personTable.Borders.Color = Colors.Black;
            _personTable.Borders.Width = 0.25;

            Column column = _personTable.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            Row row = _personTable.AddRow();
            row.Height = "0.75cm";
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Osoba:");
            row.Cells[0].MergeDown = 1;
            row.Cells[0].Borders.Width = 0;
            row.Cells[1].AddParagraph("Poz.");
            row.Cells[3].Borders.Width = 0;
            row.Cells[4].MergeRight = 2;
            row.Cells[4].Borders.Width = 0;
            row.Cells[4].AddParagraph("Numer rejestracyjny pojazdu szkoleniowego:");

            row = _personTable.AddRow();
            row.Height = "0.75cm";
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[1].AddParagraph("Książka nr");
            row.Cells[3].Borders.Width = 0;

            paragraph = section.AddParagraph();
            paragraph.Format.SpaceAfter = "0.3cm";

            _personTable2 = section.AddTable();
            _personTable2.Style = "Table";
            _personTable2.Borders.Color = Colors.Black;
            _personTable2.Borders.Width = 0.25;

            column = _personTable2.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable2.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable2.AddColumn("0.1cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable2.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable2.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable2.AddColumn("0.1cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable2.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = _personTable2.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            row = _personTable2.AddRow();
            row.Height = "0.75cm";
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Cells[0].AddParagraph("Nazwisko");
            row.Cells[0].Borders.Width = 0;
            row.Cells[1].AddParagraph(_payment.Amount.ToString());
            row.Cells[2].Borders.Width = 0;
            row.Cells[3].AddParagraph("Imię");
            row.Cells[3].Borders.Width = 0;
            row.Cells[4].AddParagraph(_payment.Amount.ToString());
            row.Cells[5].Borders.Width = 0;
            row.Cells[6].AddParagraph("Nr PESEL/data ur.");
            row.Cells[6].Borders.Width = 0;

            paragraph = section.AddParagraph();
            paragraph.Format.SpaceAfter = "0.3cm";

            // Create the item table
            _table = section.AddTable();
            _table.Style = "Table";
            _table.Borders.Color = Colors.Black;
            _table.Borders.Width = 0.25;

            // Before you can add a row, you must define the columns
            column = _table.AddColumn("0.8cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("0.8cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            row = _table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Center;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Shading.Color = Colors.LightGray;
            row.Cells[0].AddParagraph("Poz.");
            row.Cells[0].MergeDown = 1;
            row.Cells[1].AddParagraph("Data");
            row.Cells[1].MergeDown = 1;
            row.Cells[2].AddParagraph("Godzina");
            row.Cells[2].MergeRight = 1;
            row.Cells[4].AddParagraph("Liczba");
            row.Cells[4].MergeRight = 1;
            row.Cells[6].AddParagraph("T/P");
            row.Cells[6].MergeDown = 1;
            row.Cells[7].AddParagraph("Nr instruktora");
            row.Cells[7].MergeDown = 1;
            row.Cells[8].AddParagraph("Podpis instruktora");
            row.Cells[8].MergeDown = 1;
            row.Cells[9].AddParagraph("Podpis osoby szkolonej");
            row.Cells[9].MergeDown = 1;

            row = _table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Center;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Shading.Color = Colors.LightGray;
            row.Cells[2].AddParagraph("rozp.");
            row.Cells[3].AddParagraph("zak.");
            row.Cells[4].AddParagraph("godzin");
            row.Cells[5].AddParagraph("km");

            row = _table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.VerticalAlignment = VerticalAlignment.Center;
            for (var i = 0; i < _table.Columns.Count; i++)
                row.Cells[i].AddParagraph((i + 1).ToString());

            // Create footer
            paragraph = section.Footers.FirstPage.AddParagraph();
            paragraph.Format.Font.Size = 7;
            paragraph.AddText("Objaśnienia:" + "\n" +
                "1) Niepotrzebne skreślić." + "\n" +
                "2) Wpisać wszystkie numery pozycji, pod którymi osoba jest zarejestrowana w książce ewidencji osób szkolonych, i numer tej książki lub kolejny numer karty, jeżeli zajęcia prowadzone są w szkole." + "\n" +
                "3) Datę urodzenia można wpisać tylko osobm, które nie mają nadanego numeru PESEL." + "\n" +
                "4) Rubryki 2, 3 i 10 wypełnia osoba szkolona, a 4-9 instruktor - po przeprowadzonych zajęciach." + "\n" +
                "5) Wpisać liczbę przejechanych w ruchu drogowym kilometrów." + "\n" +
                "6) Wpisać rodzaj przeprowadzonych zajęć T - teoria, P - praktyka." + "\n" +
                "7) Lub podpis osoby, o której mowa w art. 8 ust. 2 ustawy z dnia 8 września 2006 r. o Państwowym Ratownictwie Medycznym (Dz. U. z 2013 r. poz. 757, z późn. zm.), lub wykładowcy.");
        }

        private void FillContent() {
            
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