using CodeDesign.Sql.Repositories;

namespace DemoForm
{
    public partial class Form1 : Form
    {
        private readonly UnitOfWork _unitOfWork;
        public Form1(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var em = _unitOfWork.EmployeeRepo.GetByID(1);
        }
    }
}