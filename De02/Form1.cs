using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using De02.model;

namespace De02
{
    public partial class Form1 : Form
    {
        private loaiSPDBcontext context; // Ngữ cảnh cơ sở dữ liệu
        private Sanpham selectedSanPham; // Sản phẩm được chọn
        

        public Form1()
        {
            InitializeComponent();
            context = new loaiSPDBcontext(); // Khởi tạo ngữ cảnh
        }

        private void FillLoaiSPComboBox(List<LoaiSP> listLoaiSP)
        {
            cbb_loaisp.DataSource = listLoaiSP;
            cbb_loaisp.DisplayMember = "TenLoai";
            cbb_loaisp.ValueMember = "MaLoai";
        }

        private void BindGrid(List<Sanpham> listSP)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listSP)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaSP;
                dataGridView1.Rows[index].Cells[1].Value = item.TenSP;
                dataGridView1.Rows[index].Cells[2].Value = item.Ngaynhap.ToString("yyyy-MM-dd"); // Định dạng ngày
                dataGridView1.Rows[index].Cells[3].Value = item.LoaiSP.TenLoai; // Kiểm tra tên loại
            }
        }

        private void ClearForm()
        {
            txt_masp.Clear();
            txt_tensp.Clear();
            cbb_loaisp.SelectedIndex = -1;
            dtpNgayNhap.Value = DateTime.Now; // Đặt lại giá trị DateTimePicker
        }

        private void LoadData()
        {
            try
            {
                List<LoaiSP> listLoaiSP = context.LoaiSP.ToList(); // Lấy danh sách loại sản phẩm
                List<Sanpham> listSP = context.Sanphams.Include("LoaiSP").ToList(); // Lấy danh sách sản phẩm
                FillLoaiSPComboBox(listLoaiSP); // Điền dữ liệu cho ComboBox
                BindGrid(listSP); // Hiển thị sản phẩm trong DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData(); // Tải dữ liệu khi form được mở
        }


        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true; // Hủy bỏ việc đóng form
            }
        }

       

        private void BtnKhongLuu_Click(object sender, EventArgs e)
        {
            ClearForm();
            MessageBox.Show("Các thay đổi đã bị hủy bỏ.");
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            try
            {
                Sanpham newSP = new Sanpham()
                {
                    MaSP = txt_masp.Text,
                    TenSP = txt_tensp.Text,
                    MaLoai = cbb_loaisp.SelectedValue.ToString(), // Gán MaLoai
                    Ngaynhap = dtpNgayNhap.Value
                };

                context.Sanphams.Add(newSP);
                context.SaveChanges();

                MessageBox.Show("Thêm sản phẩm thành công.");
                ClearForm();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            if (selectedSanPham != null)
            {
                try
                {
                    selectedSanPham.TenSP = txt_tensp.Text;
                    selectedSanPham.Ngaynhap = dtpNgayNhap.Value;
                    selectedSanPham.MaLoai = cbb_loaisp.SelectedValue.ToString(); // Gán MaLoai

                    context.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin sản phẩm thành công.");
                    ClearForm();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa.");
            }
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (selectedSanPham != null)
            {
                try
                {
                    DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm có mã số {selectedSanPham.MaSP}?", "Thông báo", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        context.Sanphams.Remove(selectedSanPham);
                        context.SaveChanges();
                        MessageBox.Show("Xóa sản phẩm thành công.");
                        ClearForm();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.");
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string SanPhamID = row.Cells[0].Value.ToString();

                selectedSanPham = context.Sanphams.FirstOrDefault(s => s.MaSP == SanPhamID);

                if (selectedSanPham != null)
                {

                    txt_masp.Text = selectedSanPham.MaSP;
                    txt_tensp.Text = selectedSanPham.TenSP;
                    cbb_loaisp.SelectedValue = selectedSanPham.MaLoai;
                    dtpNgayNhap.Value = Convert.ToDateTime(selectedSanPham.Ngaynhap);

                }
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string tenSP = txtTimKiem.Text.Trim();
            List<Sanpham> listSP;

            if (string.IsNullOrEmpty(tenSP))
            {
                listSP = context.Sanphams.Include("LoaiSP").ToList(); // Nếu không có từ khóa, lấy tất cả
            }
            else
            {
                listSP = context.Sanphams
                    .Include("LoaiSP")
                    .Where(s => s.TenSP.Contains(tenSP))
                    .ToList(); // Tìm kiếm sản phẩm theo tên
            }

            BindGrid(listSP); // Hiển thị kết quả tìm kiếm
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {

        }
    }
}