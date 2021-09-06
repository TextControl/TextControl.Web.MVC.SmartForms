using mvc_form_html.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TXTextControl;

namespace mvc_form_html.Controllers {
	public class HomeController : Controller {
		public ActionResult Index() {

			return View();
		}

		[HttpPost]
		public JsonResult CreateForm(string document) {
			using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl()) {
				tx.Create();

	   		tx.Load(Convert.FromBase64String(document), TXTextControl.BinaryStreamType.InternalUnicodeFormat);

				List<FormSection> formSections = new List<FormSection>();

				foreach (TXTextControl.SubTextPart section in tx.SubTextParts) {

					byte[] data;

					section.Save(out data, BinaryStreamType.InternalUnicodeFormat);

					var fields = GetFormFields(data);

					formSections.Add(new FormSection() {
						Name = section.Name,
						FormFields = fields
					});
				}

				return Json(formSections, JsonRequestBehavior.AllowGet);

			}

		}

		private List<SmartFormField> GetFormFields(byte[] data) {
			using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl()) {
				tx.Create();
				tx.Load(data, BinaryStreamType.InternalUnicodeFormat);

				List<SmartFormField> smartFormFields = new List<SmartFormField>();

				foreach (FormField field in tx.FormFields) {
					
					switch (field.GetType().Name) {
						case "TextFormField":
							smartFormFields.Add(new SmartTextFormField() { 
								Name = field.Name,
								Text = field.Text
							});
							break;
						case "CheckFormField":
							smartFormFields.Add(new SmartCheckboxField() {
								Name = field.Name,
								Text = field.Text,
								Checked = ((CheckFormField)field).Checked
							});
							break;
						case "SelectionFormField":
							smartFormFields.Add(new SmartDropdownField() {
								Name = field.Name,
								Text = field.Text,
								Items = ((SelectionFormField)field).Items
							});
							break;
						case "DateFormField":

							SmartDateField smartDateField = new SmartDateField() {
								Name = field.Name,
								Text = field.Text,
								Date = ""
							};

							if (((DateFormField)field).Date != null)
									smartDateField.Date = ((DateFormField)field).Date.Value.ToString("yyyy-MM-dd");

							smartFormFields.Add(smartDateField);
							break;
					}
					
				}

				return smartFormFields;
			}
		}
	}
}