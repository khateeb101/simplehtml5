﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrimsonCoward.DAL;

namespace CrimsonCoward
{
    public partial class CCHome : System.Web.UI.Page
    {

        public void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["LatestNb"] == null)
            ViewState["LatestNb"] = 3;

            List<DAL.Image> lstImages = new List<DAL.Image>();
            CrimsonCowardEntities db = new CrimsonCowardEntities();
            var sliders = db.Sliders.Where(x => x.Active).ToList();
            lstImages = (from s in db.Sliders join i in db.Images on s.ImageId equals i.Id select i).ToList();
            
            if (lstImages.Count > 0)
            {
                rptBanner.DataSource = lstImages;
                rptBanner.DataBind();
            }
              
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        protected void btnReview_Click(object sender, EventArgs e)
        {
            CrimsonCowardEntities db = new CrimsonCowardEntities();
            if(string.IsNullOrEmpty(hdnRating.Value))
            {
                lblReview.Text = "Please select star rating";
                lblReview.ForeColor = System.Drawing.Color.Red;
            }
            if (string.IsNullOrEmpty(txtReviewTitle.Text))
            {
                lblReview.Text = "Please enter your name";
                lblReview.ForeColor = System.Drawing.Color.Red;
            }
            if (string.IsNullOrEmpty(txtReviewTitle.Text))
            {
                lblReview.Text = "Please enter your review";
                lblReview.ForeColor = System.Drawing.Color.Red;
            }

            if (!string.IsNullOrEmpty(txtReviewTitle.Text) && !string.IsNullOrEmpty(txtReview.Text) && !string.IsNullOrEmpty(hdnRating.Value))
            {
                DAL.Reviews review = new Reviews
                {
                    Id = Guid.NewGuid(),
                    IsActive = false,
                    Date = DateTime.Now,
                    Text = txtReview.Text,
                    Rating = int.Parse(hdnRating.Value.Length>0?hdnRating.Value:"0"),
                    Title = txtReviewTitle.Text
                };
                db.Reviews.Add(review);
                db.SaveChanges();
                lblReview.Text = "Thank you for your review";
                lblReview.ForeColor = System.Drawing.Color.Green;
                txtReview.Text = "";
                txtReviewTitle.Text = "";
            }
        }
        protected void btnSubscribe_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSubscribe.Text))
            {
                if (IsValidEmail(txtSubscribe.Text))
                {
                    CrimsonCowardEntities db = new CrimsonCowardEntities();
                    Subscriber subsc = new Subscriber();
                    if (db.Subscribers.Where(x => x.email.ToLower() == txtSubscribe.Text.ToLower()).Count() > 0)
                    {
                        lblSubscribe.Text = "Dear " + txtSubscribe.Text + ". Your Email already exists";
                        lblSubscribe.ForeColor = System.Drawing.Color.Yellow;
                        return;
                    }
                    db.Subscribers.Add(new Subscriber() { email = txtSubscribe.Text });
                    db.SaveChanges();
                    lblSubscribe.Text = "Dear " + txtSubscribe.Text + ". Kindly note that your Email has been successfully added!";
                    lblSubscribe.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    lblSubscribe.Text = "Please enter a valid email address!";
                    lblSubscribe.ForeColor = System.Drawing.Color.Yellow;
                }

            }

        }
    }
}