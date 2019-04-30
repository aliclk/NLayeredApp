using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.Ninject;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebFormsUI
{
    public partial class ProductsForm : Form
    {
        public ProductsForm()
        {
            InitializeComponent();
            //_productservice = new ProductManager(new EfProductDal());
            _productservice = InstanceFactory.GetInstance<IProductService>();
            //_categoryservice = new CategoryManager(new EfCategoryDal());
            _categoryservice = InstanceFactory.GetInstance<ICategorService>();
        }
        private IProductService _productservice;
        private ICategorService _categoryservice;
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCategories();         
        }

        private void LoadCategories()
        {
            cbxCategory.DataSource = _categoryservice.GetAll();
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryId";

            cbxCategory2.DataSource = _categoryservice.GetAll();
            cbxCategory2.DisplayMember = "CategoryName";
            cbxCategory2.ValueMember = "CategoryId";

            cbxCategoryUpdate.DataSource = _categoryservice.GetAll();
            cbxCategoryUpdate.DisplayMember = "CategoryName";
            cbxCategoryUpdate.ValueMember = "CategoryId";
        }

        private void LoadProducts()
        {
            dgwProduct.DataSource = _productservice.GetAll();
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgwProduct.DataSource = _productservice.GetProductsByCategory(Convert.ToInt32(cbxCategory.SelectedValue));
            }
            catch (Exception)
            {

            }          
        }

        private void tbxProductName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxProductName.Text))
            {
                dgwProduct.DataSource = _productservice.GetProductsByProductName(tbxProductName.Text);
            }
            else
            {
                LoadProducts();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _productservice.Add(new Product
                {
                    CategoryId = Convert.ToInt32(cbxCategory2.SelectedValue),
                    ProductName = tbxUrunAdi2.Text,
                    QuantityPerUnit = tbxQuantity.Text,
                    UnitPrice = Convert.ToDecimal(tbxUnitPrice.Text),
                    UnitsInStock = Convert.ToInt16(tbxStock.Text)
                });
                MessageBox.Show("Yeni Ürün Eklendi");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }         
        }

        private void btnUrunGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                _productservice.Update(new Product
                {
                    ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value),
                    ProductName = tbxProductNameUpdate.Text,
                    CategoryId = Convert.ToInt32(cbxCategory2.SelectedValue),
                    UnitsInStock = Convert.ToInt16(TbxStockUpdate.Text),
                    QuantityPerUnit = TbxQuantityUpdate.Text,
                    UnitPrice = Convert.ToDecimal(TbxUnitPriceUpdate.Text)
                });

                MessageBox.Show("Ürün Güncellendi");
                LoadProducts();
            }
            catch (Exception)
            {
                MessageBox.Show("Hata! Ürün güncellenemedi");
            }
            
        }

        private void dgwProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //seçilen ürün update için kutucukların dolması gerekir.
            tbxProductNameUpdate.Text = dgwProduct.CurrentRow.Cells[1].Value.ToString();
            cbxCategoryUpdate.SelectedValue = dgwProduct.CurrentRow.Cells[2].Value;
            TbxUnitPriceUpdate.Text = dgwProduct.CurrentRow.Cells[3].Value.ToString();
            TbxQuantityUpdate.Text = dgwProduct.CurrentRow.Cells[4].Value.ToString();
            TbxStockUpdate.Text = dgwProduct.CurrentRow.Cells[5].Value.ToString();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgwProduct.CurrentRow != null)
                {
                    _productservice.Delete(new Product
                    {
                        ProductId = Convert.ToInt32(dgwProduct.CurrentRow
                   .Cells[0].Value)
                    });
                    MessageBox.Show("Ürün Silme işlemi başarılı");
                    LoadProducts();
                }               
            }
            catch (Exception)
            {
                MessageBox.Show("Ürün Silinemedi");
            }            
        }       
    }
}
