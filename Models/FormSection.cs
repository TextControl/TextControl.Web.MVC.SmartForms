using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TXTextControl;

namespace mvc_form_html.Models {
	public class FormSection {
		public string Name { get; set; }
		public List<SmartFormField> FormFields { get; set; }
	}

	public class SmartFormField {
		public string Name { get; set; }
		public string Text { get; set; }
	}

	public class SmartTextFormField : SmartFormField {
		public string TypeName { get; set; } = "SmartTextFormField";
	}

	public class SmartCheckboxField : SmartFormField {
		public bool Checked { get; set; }
		public string TypeName { get; set; } = "SmartCheckboxField";
	}

	public class SmartDropdownField : SmartFormField {
		public string[] Items { get; set; }
		public string TypeName { get; set; } = "SmartDropdownField";
	}

	public class SmartDateField : SmartFormField {
		public string Date { get; set; }
		public string TypeName { get; set; } = "SmartDateField";
	}

}