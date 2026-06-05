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
                PagePadding = new Thickness(80, 60, 80, 60),
                FontFamily = new FontFamily("Arial"),
                FontSize = 13,
                LineHeight = 22,
                TextAlignment = TextAlignment.Left,
                PageWidth = 816,  // A4 width in pixels (96dpi)
                ColumnWidth = double.MaxValue  // Quan trọng - tắt multi-column
            };

            // Tiêu đề bệnh viện
            doc.Blocks.Add(new Paragraph(new Run("BỆNH VIỆN ĐA KHOA"))
            {
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 6)
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
            var signTable = new Table { Margin = new Thickness(0, 60, 0, 0) };
            signTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            signTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });

            var signRowGroup = new TableRowGroup();

            // Dòng 1: ngày tháng
            var row1 = new TableRow();
            row1.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            row1.Cells.Add(new TableCell(new Paragraph(
                new Run($"Ngày {record.CreatedDate:dd} tháng {record.CreatedDate:MM} năm {record.CreatedDate:yyyy}"))
            {
                TextAlignment = TextAlignment.Center
            }));
            signRowGroup.Rows.Add(row1);

            // Dòng 2: chức danh
            var row2 = new TableRow();
            row2.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            row2.Cells.Add(new TableCell(new Paragraph(
                new Run("BÁC SĨ KHÁM BỆNH"))
            {
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 4, 0, 0)
            }));
            signRowGroup.Rows.Add(row2);

            // Dòng 3: ký tên
            var row3 = new TableRow();
            row3.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            row3.Cells.Add(new TableCell(new Paragraph(
                new Run("(Ký và ghi rõ họ tên)"))
            {
                TextAlignment = TextAlignment.Center,
                Foreground = Brushes.Gray,
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(0, 4, 0, 40)
            }));
            signRowGroup.Rows.Add(row3);

            // Dòng 4: tên bác sĩ
            var row4 = new TableRow();
            row4.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            row4.Cells.Add(new TableCell(new Paragraph(
                new Run(record.Appointment?.Doctor?.FullName ?? ""))
            {
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold
            }));
            signRowGroup.Rows.Add(row4);

            signTable.RowGroups.Add(signRowGroup);
            doc.Blocks.Add(signTable);

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