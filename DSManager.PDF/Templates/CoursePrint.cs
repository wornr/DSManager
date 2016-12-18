using System;
using System.Diagnostics;
using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

using PdfSharp.Pdf;

using DSManager.Model.Entities;
using DSManager.Model.Enums;
using DSManager.PDF.Utilities;

namespace DSManager.PDF.Templates {
    public class CoursePrint {
        private Font font = new Font("Arial", 10);
        private Participant participant;

        public CoursePrint(Participant participant) {
            this.participant = participant;

            Document document = new Document();
            document.Info.Author = "Rolf Baxter";
            document.Info.Keywords = "MigraDoc, Examples, C#";

            // Get the A4 page size
            Unit width, height;
            PageSetup.GetPageSize(PageFormat.A4, out width, out height);

            // Add a section to the document and configure it such that it will be in the centre
            // of the page
            Section section = document.AddSection();
            section.PageSetup.PageHeight = height;
            section.PageSetup.PageWidth = width;
            section.PageSetup.TopMargin = new Unit(8, UnitType.Millimeter);
            section.PageSetup.LeftMargin = new Unit(8, UnitType.Millimeter);

            Table topSection = new Table();
            Table table = new Table();

            GenerateTopSection(topSection);
            GenerateTableHeader(table);
            GenerateTableRows(table, 50);

            // Add a row with a single cell for the second line
            /*row = table.AddRow();
            cell = row.Cells[0];

            cell.Format.Font.Color = Colors.Black;
            cell.Format.Alignment = ParagraphAlignment.Left;
            cell.Format.Font.ApplyFont(font);

            cell.AddParagraph("This is some long text that *will* auto-wrap when the edge of the page is reached");*/

            document.LastSection.Add(topSection);
            document.LastSection.AddParagraph(
                "w zakresie prawa jazdy kategorii ............../pozwolenia na kursie dla kandydatów na kierowców lub motorniczych");
            document.LastSection.Add(table);

            // Create a renderer
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            // Save and show the document
            var fileName =
                participant.Student.FirstName + " " +
                participant.Student.SecondName + " " +
                participant.Student.LastName + " - " +
                AddLeadingZeros.Convert(participant.Course.StartDate.Day) + "." +
                AddLeadingZeros.Convert(participant.Course.StartDate.Month) + "." +
                AddLeadingZeros.Convert(participant.Course.StartDate.Year) +
                ".pdf";

            try {
                pdfRenderer.PdfDocument.Save(fileName);
                Process.Start(fileName);
            } catch (Exception e) {
                throw;
            }
        }

        private void GenerateTopSection(Table topSection) {
            topSection.Borders.Width = 1;

            var column = topSection.AddColumn(new Unit(75, UnitType.Millimeter));
            column.Format.Alignment = ParagraphAlignment.Center;
            column = topSection.AddColumn(new Unit(60, UnitType.Millimeter));
            column.Format.Alignment = ParagraphAlignment.Center;
            column = topSection.AddColumn(new Unit(75, UnitType.Millimeter));
            column.Format.Alignment = ParagraphAlignment.Left;

            var row = topSection.AddRow();
            row.Height = new Unit(15, UnitType.Millimeter);
            topSection.AddRow();

            topSection.Rows[0].Cells[0].Borders.Visible = false;
            topSection.Rows[1].Cells[1].Borders.Visible = false;
            topSection.Rows[1].Cells[2].Borders.Visible = false;

            var cell = topSection.Rows[1].Cells[0];
            cell.Format.Font.Color = Colors.Black;
            cell.Format.Font.ApplyFont(new Font("Arial", 8));
            cell.AddParagraph("................................................................................" +
                            "\n(nazwa ośrodka szkolenia kierowców wydającego kartę)");
            cell.Borders.Visible = false;

            cell = topSection.Rows[0].Cells[1];
            cell.Format.Font.Color = Colors.Black;
            cell.Format.Font.ApplyFont(new Font("Arial", 16));
            cell.AddParagraph("Karta przeprowadzonych zajęć");
            cell.MergeDown = 1;

            cell = topSection.Rows[0].Cells[2];
            cell.Format.Font.Color = Colors.Black;
            cell.Format.Font.ApplyFont(font);
            cell.AddParagraph("Data wydania .......................................");
            cell.Borders.Visible = false;
        }

        private void GenerateTableHeader(Table table) {
            // Create a table so that we can draw the horizontal lines
            table.Borders.Width = 1;

            var columnsWidth = new[] { 8, 25, 15, 15, 15, 10, 8, 30, 35, 35 };
            // Default to show borders 1 pixel wide Column
            foreach(var columnWidth in columnsWidth) {
                var column = table.AddColumn(new Unit(columnWidth, UnitType.Millimeter));
                column.Format.Alignment = ParagraphAlignment.Center;
            }

            // Add a row with a single cell for the first line
            table.AddRow();
            table.AddRow();

            var skipNext = false;
            var offset = 0;

            var columnHeaders = new[] { "Poz.", "Data", "Godzina", "Liczba", "T/P", "Nr instruktora", "Podpis instruktora", "Podpis osoby szkolonej" };
            var columnSubHeaders = new[] { "rozp.", "zak.", "godzin", "km" };
            var j = 0;
            for(var i = 0; i < columnsWidth.Length; i++) {
                Cell cell;

                if(i > 1 && i < 6) {
                    cell = table.Rows[1].Cells[i];
                    cell.Format.Font.Color = Colors.Black;
                    cell.Format.Font.ApplyFont(font);
                    cell.AddParagraph(columnSubHeaders[j++]);
                }

                if(skipNext) {
                    skipNext = false;
                    offset++;
                    continue;
                }

                cell = table.Rows[0].Cells[i];
                cell.Format.Font.Color = Colors.Black;
                cell.Format.Font.ApplyFont(font);
                cell.AddParagraph(columnHeaders[i - offset]);

                if(i == 2 || i == 4) {
                    cell.MergeRight = 1;
                    skipNext = true;
                }

                if(i < 2 || i > 5) {
                    cell.MergeDown = 1;
                }
            }
        }

        private void GenerateTableRows(Table table, int rows) {
            for (var i = 0; i < rows; i++) {
                var row = table.AddRow();
                var cell = row.Cells[0];

                row.Height = new Unit(8, UnitType.Millimeter);

                cell.AddParagraph((i+1).ToString());
            }

            var rowNr = 2;
            foreach (var classInfo in participant.ClassesDates.OrderBy(x => x.StartDate)) {
                var timeDifference = classInfo.EndDate == null ? new TimeSpan() : (TimeSpan)(classInfo.EndDate - classInfo.StartDate);

                table.Rows[rowNr].Cells[1].AddParagraph(AddLeadingZeros.Convert(classInfo.StartDate.Day) + "." + AddLeadingZeros.Convert(classInfo.StartDate.Month) + "." + AddLeadingZeros.Convert(classInfo.StartDate.Year));
                table.Rows[rowNr].Cells[2].AddParagraph(AddLeadingZeros.Convert(classInfo.StartDate.Hour) + ":" + AddLeadingZeros.Convert(classInfo.StartDate.Minute));
                table.Rows[rowNr].Cells[3].AddParagraph(AddLeadingZeros.Convert(classInfo.EndDate?.Hour) + ":" + AddLeadingZeros.Convert(classInfo.EndDate?.Minute));
                table.Rows[rowNr].Cells[4].AddParagraph(AddLeadingZeros.Convert(timeDifference.Hours) + ":" + AddLeadingZeros.Convert(timeDifference.Minutes));
                table.Rows[rowNr].Cells[5].AddParagraph(classInfo.Distance == null ? "" : classInfo.Distance.ToString());
                table.Rows[rowNr].Cells[6].AddParagraph(classInfo.CourseKind == CourseKind.Theory ? "T" : "P");
                table.Rows[rowNr].Cells[7].AddParagraph(classInfo.Instructor.PermissionsNr);
                rowNr++;
            }
        }
    }
}
