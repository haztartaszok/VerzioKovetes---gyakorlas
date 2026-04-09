using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance.Properties;
using UserMaintenance.Entities;
using System.IO;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();
        public Form1()
        {
            InitializeComponent();
            lblFullName.Text = Resources.FullName; // label1
            btnAdd.Text = Resources.Add; // button1
            btnWrite.Text = Resources.Write; // button2
            btnDelete.Text = Resources.Delete;

            // attach write handler
            this.btnWrite.Click += this.btnWrite_Click;

            // listbox
            listUsers.DataSource = users;
            listUsers.ValueMember = "ID";
            listUsers.DisplayMember = "FullName";

            // attach delete handler
            this.btnDelete.Click += this.btnDelete_Click;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var u = new User()
            {
                FullName = txtFullName.Text
            };
            users.Add(u);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selected = listUsers.SelectedItem as User;
            if (selected == null) return;

            users.Remove(selected);
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.DefaultExt = "txt";
                sfd.AddExtension = true;
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // header (tab-separated)
                        sw.WriteLine("ID\tFullName");
                        foreach (var u in users) // lista elemenként megnézem, hogy van-e benne tab, idézőjel, új sor, stb., és ha igen, akkor megfelelően escape-elem
                        {
                            string Escape(string v)
                            {
                                if (v == null) return "";
                                var escaped = v.Replace("\"", "\"\"");
                                if (escaped.Contains("\t") || escaped.Contains("\"") || escaped.Contains("\n") || escaped.Contains("\r"))
                                    return "\"" + escaped + "\"";
                                return escaped;
                            }

                            sw.WriteLine($"{u.ID}\t{Escape(u.FullName)}");
                        }
                    }

                    MessageBox.Show(this, "Fájl mentve.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Hiba a fájl mentésekor: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
