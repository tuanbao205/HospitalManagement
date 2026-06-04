using HospitalManagement.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace HospitalManagement.Helpers
{
    public static class PrintHelper
    {
        public static void PrintMedicalRecord(MedicalRecord record)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() != true) return;

            var document = CreateDocument(record);
            printDialog.PrintDocument(
                ((IDocumentPaginatorSource)document).DocumentPaginator,
                "Hồ sơ bệnh án");
        }

        private static FlowDocument CreateDocument(MedicalRecord record)
        {
            var doc = new FlowDocument
            {
                PagePadding = new Thickness(60),
                FontFamily = new FontFamily("Arial"),
                FontSize = 13
            };

            // Tiêu đề bệnh viện
            doc.Blocks.Add(new Paragraph(new Run("BỆNH VIỆN ĐA KHOA"))
            {
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            });

            doc.Blocks.Add(new Paragraph(new Run("HỒ SƠ BỆNH ÁN"))
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            });

            // Đường kẻ ngang
            doc.Blocks.Add(new BlockUIContainer(new System.Windows.Shapes.Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = 700,
                Y2 = 0,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            }));

            // Thông tin bệnh nhân
            doc.Blocks.Add(CreateSection("THÔNG TIN BỆNH NHÂN"));
            doc.Blocks.Add(CreateRow("Họ và tên:", record.Appointment?.Patient?.FullName));
            doc.Blocks.Add(CreateRow("Ngày sinh:", record.Appointment?.Patient?.DOB.ToString("dd/MM/yyyy")));
            doc.Blocks.Add(CreateRow("Giới tính:", record.Appointment?.Patient?.Gender));
            doc.Blocks.Add(CreateRow("Số điện thoại:", record.Appointment?.Patient?.Phone));
            doc.Blocks.Add(CreateRow("Địa chỉ:", record.Appointment?.Patient?.Address));

            // Thông tin khám
            doc.Blocks.Add(CreateSection("THÔNG TIN KHÁM"));
            doc.Blocks.Add(CreateRow("Bác sĩ khám:", record.Appointment?.Doctor?.FullName));
            doc.Blocks.Add(CreateRow("Chuyên khoa:", record.Appointment?.Doctor?.Specialty));
            doc.Blocks.Add(CreateRow("Ngày khám:", record.Appointment?.Date.ToString("dd/MM/yyyy HH:mm")));
            doc.Blocks.Add(CreateRow("Ngày lập hồ sơ:", record.CreatedDate.ToString("dd/MM/yyyy HH:mm")));

            // Kết quả khám
            doc.Blocks.Add(CreateSection("KẾT QUẢ KHÁM"));
            doc.Blocks.Add(CreateRow("Chẩn đoán:", record.Diagnosis));
            doc.Blocks.Add(CreateRow("Đơn thuốc:", record.Prescription));
            doc.Blocks.Add(CreateRow("Ghi chú:", record.Note));

            // Chữ ký
            doc.Blocks.Add(new Paragraph()
            {
                Margin = new Thickness(0, 40, 0, 0)
            });

            var signTable = new Table();
            signTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            signTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });

            var rowGroup = new TableRowGroup();
            var row = new TableRow();

            var cellDate = new TableCell(new Paragraph(
                new Run($"Ngày {record.CreatedDate:dd} tháng {record.CreatedDate:MM} năm {record.CreatedDate:yyyy}")))
            {
                TextAlignment = TextAlignment.Center
            };

            var cellSign = new TableCell(new Paragraph(
                new Run("Bác sĩ khám bệnh")))
            {
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold
            };

            row.Cells.Add(cellDate);
            row.Cells.Add(cellSign);
            rowGroup.Rows.Add(row);
            signTable.RowGroups.Add(rowGroup);
            doc.Blocks.Add(signTable);

            // Tên bác sĩ ký
            var signRow = new TableRow();
            var signName = new TableCell(new Paragraph(
                new Run(record.Appointment?.Doctor?.FullName ?? "")))
            {
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold
            };
            var emptyCell = new TableCell(new Paragraph(new Run("")));

            var signRowGroup2 = new TableRowGroup();
            var signTable2 = new Table();
            signTable2.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            signTable2.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            var signRow2 = new TableRow();
            signRow2.Cells.Add(emptyCell);
            signRow2.Cells.Add(signName);
            signRowGroup2.Rows.Add(signRow2);
            signTable2.RowGroups.Add(signRowGroup2);

            var nameParag = new Paragraph(
                new Run($"\n\n{record.Appointment?.Doctor?.FullName}"))
            {
                TextAlignment = TextAlignment.Right,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 40, 0, 0)
            };
            doc.Blocks.Add(nameParag);

            return doc;
        }

        private static Paragraph CreateSection(string title)
        {
            return new Paragraph(new Run(title))
            {
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 20, 0, 8),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(0, 0, 0, 4)
            };
        }

        private static Paragraph CreateRow(string label, string value)
        {
            var para = new Paragraph();
            para.Inlines.Add(new Run(label) { FontWeight = FontWeights.Bold });
            para.Inlines.Add(new Run("  " + (value ?? "Chưa có thông tin")));
            para.Margin = new Thickness(0, 2, 0, 2);
            return para;
        }
    }
}