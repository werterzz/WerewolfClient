using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WerewolfClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mMainForm = new MainForm();
            mMainForm.Visible = false;
            Login mLogin = new Login(mMainForm);
            register mRegister = new register(mLogin);
            Menu mMenu = new Menu(mMainForm);
            WerewolfController mControler =  WerewolfController.GetInstance();
            WerewolfModel mModel = new WerewolfModel();


            // View -> Controller
            mMainForm.setController(mControler);
            mLogin.setController(mControler);
            mRegister.setController(mControler);
            mMenu.setController(mControler);
            // Controler -> Model
            mControler.AddModel(mModel);

            // Model -> View
            mModel.AttachObserver(mLogin);
            mModel.AttachObserver(mMainForm);

            Application.Run(mLogin);
        }
    }
}
