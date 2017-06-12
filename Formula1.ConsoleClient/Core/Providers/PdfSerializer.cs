namespace Formula1.ConsoleClient.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Contracts;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public class PdfSerializer : ISerializer
    {
        private ILogger logger;
        private IWriter writer;

        public PdfSerializer(IWriter writer, ILogger logger)
        {
            this.writer = writer;
            this.logger = logger;
        }
        public void Export(string path, IDictionary<string, string> list, IEnumerable<string> headers)
        {
            PdfDocument pdfDoc = new PdfDocument();
            Document ExportDoc = new Document();

            string fileName = path + "/F1PDF_" + DateTime.Now.ToString("s").Replace(":", "") + ".pdf";

            PdfWriter.GetInstance(ExportDoc, new FileStream(fileName, FileMode.Create));
            ExportDoc.Open();


            PdfPTable table = new PdfPTable(3);
            table.SetWidths(new float[] { 1, 7, 7 });


            PdfPCell firstcell = new PdfPCell(new Phrase());
            firstcell.Border = Rectangle.BOTTOM_BORDER;
            table.AddCell(firstcell);

            foreach (var item in headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(item));
                cell.Border = Rectangle.BOTTOM_BORDER;
                table.AddCell(cell);
            }

            table.CompleteRow();

            int i = 0;
            foreach (var item in list)
            {
                i++;

                PdfPCell cell = new PdfPCell(new Phrase(i.ToString() + '.'));
                cell.Border = Rectangle.NO_BORDER;
                table.AddCell(cell);

                PdfPCell cell1 = new PdfPCell(new Phrase(item.Key));
                cell1.Border = Rectangle.LEFT_BORDER;
                table.AddCell(cell1);


                PdfPCell cell2 = new PdfPCell(new Phrase(item.Value));
                cell2.Border = Rectangle.LEFT_BORDER;
                table.AddCell(cell2);

                table.CompleteRow();
            }

            ExportDoc.Add(table);

            ExportDoc.Close();

            this.writer.WriteLine($"Export successful: {fileName}");
            this.logger.Info($"Export successful: {fileName}");
        }
    }
}