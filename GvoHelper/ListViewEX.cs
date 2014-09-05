using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GvoHelper
{
    public class ListViewEx : ListView
    {
        private TextBox m_tb;

        public ListViewEx()
        {
            m_tb = new TextBox();
            m_tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            m_tb.Multiline = true;
            m_tb.Visible = false;
            //GridLines = true;
            //this.FullRowSelect = true;
            Controls.Add(m_tb);
        }

        private void EditItem(ListViewItem.ListViewSubItem subItem)
        {
            if (this.SelectedItems.Count <= 0)
            {
                return;
            }

            Rectangle _rect = subItem.Bounds;
            m_tb.Bounds = _rect;
            m_tb.BringToFront();
            m_tb.Text = subItem.Text;
            m_tb.Leave += new EventHandler(tb_Leave);
            m_tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
            m_tb.TextChanged += new EventHandler(m_tb_TextChanged);
            m_tb.Visible = true;
            m_tb.Tag = subItem;
            m_tb.Select();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x115 || m.Msg == 0x114)
            {
                this.m_tb.Visible = false;
            }
            base.WndProc(ref m);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            this.m_tb.Visible = false;
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            Point tmpPoint = this.PointToClient(Cursor.Position);
            ListViewItem.ListViewSubItem subitem = this.HitTest(tmpPoint).SubItem;
            ListViewItem item = this.HitTest(tmpPoint).Item;
            if (!item.Checked)
                item.Checked = true;
            if (subitem != null && !item.SubItems[0].Equals(subitem))
            {
                if (item.SubItems.Count > 3)
                {
                    if (subitem == item.SubItems[3])
                        EditItem(subitem);
                }
                else
                {
                    if (subitem == item.SubItems[2])
                        EditItem(subitem);
                }

            }
            base.OnDoubleClick(e);
        }

        private void tb_Leave(object sender, EventArgs e)
        {
            m_tb.TextChanged -= new EventHandler(m_tb_TextChanged);
            (sender as TextBox).Visible = false;
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(int)Keys.Escape || e.KeyChar == (char)(int)Keys.Enter)
            {
                //m_tb.TextChanged -= new EventHandler(m_tb_TextChanged);
                (sender as TextBox).Visible = false;
            }
        }

        private void m_tb_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Tag is ListViewItem.ListViewSubItem)
                (this.m_tb.Tag as ListViewItem.ListViewSubItem).Text = this.m_tb.Text;
        }
    }
}

