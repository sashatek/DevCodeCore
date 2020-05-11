using DevCodeCore.Models;
using DevCodeCore.Shared;
using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders
{
    class BaseCoder
    {
        public string message { get; set; }
        protected CodeWriter writer = new CodeWriter();
        protected BaseCoder()
        {
            message = "Test message from lib";
        }
        protected string replaceNames(EntityModel defs, string s)
        {
            return s.Replace("Trip", defs.entityName)
                .Replace("trip", defs.entityNameLower)
                .Replace("DevCode", defs.dbContext)
                .Replace("Airport", defs.control.controlName)
                .Replace("airport", defs.control.controlNameLower); 
        }
        public string makeFormGroup(EntityModel defs, int nest)
        {
            var writer = new CodeWriter();
            writer.nest(nest);
            for (int i = 0; i < defs.fieldDefs.Count; i++)
            {
                var field = defs.fieldDefs[i];
                if (field.showOnForm)
                {
                    var validator = field.required ? ", Validators.required" : "";
                    var comma = i == defs.fieldDefs.Count - 1 ? "" : ", ";
                    if (field.refDataType == 2)
                    {
                        writer.writeLine($"{field.fieldNameLower2}: [model.{field.fieldNameLower2}{validator}]{comma}");
                    }
                    else
                    // if (field.editable)
                    {
                        writer.writeLine($"{field.fieldNameLower}: [model.{field.fieldNameLower}{validator}]{comma}");
                    }
                }
            }
            return writer.toString();
        }
        public string makeFormGetValue(EntityModel defs, int nest)
        {
            var writer = new CodeWriter();
            writer.nest(nest);
            foreach (var field in defs.fieldDefs)
            {
                if (field.showOnForm)
                {
                    if (field.refDataType == 2)
                    {
                        writer.writeLine($"model.{field.fieldNameLower2} = form.controls.{field.fieldNameLower2}.value;");
                    }
                    else
                    {
                        writer.writeLine($"model.{field.fieldNameLower} = form.controls.{field.fieldNameLower}.value;");
                        if (field.controlType == ControlType.Dropdown && field.refDataType == 1)
                        {
                            //this.model.transTypeDesc = this.refDataService.getRefDataById(this.refDataService.refData.transTypes, this.model.transTypeId).text;
                            writer.writeLine($"model.{field.fieldNameLower2} = this.refDataService.getRefDataById(this.refDataService.refData.{field.operandLower1}s, model.{field.fieldNameLower}).text;");
                        }
                    }
                }
            }
            return writer.toString();
        }

        protected string codeHtmlControl(FieldModel field, string entityName, bool forForm = false)
        {
            var html = "";
            var name = field.fieldNameLower;
            var name2 = field.fieldNameLower2;
            var service = field.operand1;
            var serviceLower = field.operandLower1;
            var required = field.isNullable ? "" : "required";

            if (field.controlLink != null)
            {
                var tagName = field.controlLink.tagName;
                var controlName = field.controlLink.controlName;
                html = $@"<{tagName} id=""{name2}"" (on{controlName}Select)=""arptSelect($event)"" 
    [parentForm]=""{entityName}Form"" [formFieldName]=""'{name2}'"">
</{tagName}>";
                return html;
            }

            switch (field.controlType)
            {
                case ControlType.Text:
                    html = $@"<input id=""{name}"" type=""text"" class=""form-control"" formControlName=""{name}"" {required}>";
                    break;
                case ControlType.TextArea:
                    html = $@"<textarea id=""{name}"" type = ""text"" class=""form-control"" formControlName=""{name}"" rows=""3"" {required}></textarea>";
                    break;
                case ControlType.Date:
                    break;
                case ControlType.Email:
                    html = $@"<input id=""{name}"" type=""email"" class=""form-control"" formControlName=""{name}"" {required}>";
                    break;
                case ControlType.DatePicker:
                    html = $@"<div class=""input-group"">
    <input id=""{name}"" type=""text"" ngbDatepicker #d=""ngbDatepicker"" class=""form-control date-box""
        formControlName=""{name}"" {required} [displayMonths]=""'2'"">
    <div class=""input-group-append"">
        <button class=""btn btn-outline-secondary calendar"" (click)=""d.toggle()"" type=""button""></button>
    </div>
</div>";
                    break;
                case ControlType.TimePicker:
                    break;
                case ControlType.Dropdown:
                    html = $@"<select id=""{name}"" class=""form-control"" id=""transType"" formControlName=""{name}"" {required}>
    <option *ngFor=""let tran of refDataService.refData.{serviceLower}s"" [ngValue]=""tran.id"">{{{{tran.text}}}}</option>
</select>";
                    break;

                case ControlType.TypeAhead:
                    html = "";
                    break;

                case ControlType.TypeAheadSvc:
                    html = $@"<input id=""{name}"" typeahead-http"" type=""text"" class=""form-control"" formControlName=""{name}""  [ngbTypeahead]=""{service}""
    (selectItem)=""select($event)"" [inputFormatter]=""formatter"" [resultFormatter]=""formatterr""
    onfocus=""this.select();"" onmouseup=""return false;"" {required}/>";
                    break;
                case ControlType.Number:
                    html = $@"<input id=""{name}"" type=""number"" class=""form-control"" formControlName=""{name}"" {required}>";
                    break;
                case ControlType.CheckBox:
                    var l = forForm ? field.label : null;
                    html = $@"<div class=""form-check"">
    <input id=""{name}"" type=""checkbox"" class=""form-check-input"" formControlName=""{name}"">
    <label class=""form-check-label"" for=""{name}"">{l}</label>
</div>";
                    break;
                case ControlType.Component:
                    html = $@"<{service} id=""{name}"" [model]=""model.{name}"" (onArptSelect)=""arptSelect($event)"" 
    [parentForm]=""{entityName}Form"" [formFieldName]=""""{name}"""">
</{service}>";
                    break;

            }

            return html;
        }

    }
}
