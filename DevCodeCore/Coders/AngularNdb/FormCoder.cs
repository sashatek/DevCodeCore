using DevCodeCore.Models;
using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.AngularNdb
{
    class FormCoder : BaseCoder
    {
        public Snippet[] code(EntityModel defs)
        {
            List<Snippet> snippets = new List<Snippet>();
            snippets.Add(codeController(defs));
            snippets.Add(codeHtml(defs));
            return snippets.ToArray();
        }
        public Snippet codeController(EntityModel defs)
        {
            var template = @"
@Component({
  selector: 'app-trip-form',
  templateUrl: './trip-form.component.html',
  styleUrls: ['./trip-form.component.css'],
  providers: [
    { provide: NgbDateAdapter, useClass: NgbUTCStringAdapter },
    { provide: NgbDateParserFormatter, useClass: NgbDateFRParserFormatter }
  ]
})

export class TripFormComponent implements OnInit {
  get model() {return this._model; }
  @Input() set model(value: TripModel) {
    this._model = value;
    if (value !== null) {
      this.initForm(this.model);
    }
  }

  @Input() enableDelete = false;
  @Input() showButtons = true;
  // tslint:disable-next-line: no-output-on-prefix
  @Output() save = new EventEmitter<TripModel>();
  @Output() cancel = new EventEmitter<TripModel>();
  @Output() delete = new EventEmitter<TripModel>();

  private _model: TripModel = new TripModel();

  arpt: ILookupItem = null;

  searching = false;
  searchFailed = false;
  tripForm: FormGroup = null;

  constructor(private tripService: TripService,
              private arptService: ArptLookupService,
              public refDataService: RefDataService,
              private fb: FormBuilder) {
    this.initForm(this.model);
  }

  ngOnInit() {
    const a = this.enableDelete;
  }

  // ngOnChanges(changes: SimpleChanges): void {
  //   if(changes['model']) {
  //     this.initForm(this.model);
  //   }
  // }
  setModel(model: TripModel) {
    this.model = model;
    this.initForm(this.model);
    // this.tripForm.reset(model);
  }
  private initForm(model:TripModel) {
    this.tripForm = this.fb.group({
$$assign1$$
    });
  }

  saveTrip() {
    // this.model = {...this.model, ...this.tripForm.value};
    this.updateModel(this.model, this.tripForm);
    this.initForm(this.model);

    // model.airportInfo = {...this.tripForm.value.airportInfo};
    this.save.emit(this.model);
    // this.tripForm.reset(this.model); TODO: check whether reset is needed
    // Object.keys(this.tripForm.controls).forEach(control => {
    //   this.tripForm.controls[control].markAsPristine();
    // });
  }

  updateModel(model: TripModel, form: FormGroup) {
$$assign2$$
  }

  isDitry() {
    return this.tripForm.dirty;
  }
  isValid() {
    return this.tripForm.valid;
  }
  cancelTrip() {
    this.cancel.emit(this.model);
    this.initForm(this.model);
  }

  deleteTrip() {
    this.delete.emit(this.model);
  }

  arptSelect(item: ILookupItem) {
    this.arpt = item;
  }

  onTransTypeChange(event, model: TripModel) {
    model.transTypeDesc = event.target.options[event.target.options.selectedIndex].text;
  }
}

";

            var snippet = new Snippet();
            snippet.header = "Form Controller";
            snippet.language = Language.TypeScript;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            var assign1 = makeFormGroup(defs, 3);
            var assign2 = makeFormGetValue(defs, 2);
            snippet.code = snippet.code
                 .Replace("$$assign1$$", assign1)
                 .Replace("$$assign2$$", assign2);
            return snippet;
        }

        public Snippet codeHtml(EntityModel defs)
        {
            string startTemplate = @"
<form [formGroup]=""tripForm"" novalidate>
    <div>";

            string endTemplate = @"    </div>
    <div class=""pull-left"" *ngIf=""showButtons"">
        <button type=""button"" class=""btn btn-primary mr-2"" (click)=""saveTrip()""
            [disabled]=""!(tripForm.dirty && tripForm.valid)"">
            Save
        </button>
        <button type=""button"" class=""btn btn-secondary mr-2"" (click)=""cancelTrip()"">Cancel</button>
        <button type=""button"" class=""btn btn-warning"" (click)=""deleteTrip()"" *ngIf=""model && !model.isNew && enableDelete"">
            Delete
        </button>

    </div>
</form>";


            //            var endtemplate = @"

            //";

            var writer = new CodeWriter();
            var snippet = new Snippet();
            snippet.header = "Form Content and Layout";
            snippet.language = Language.HTML;
            snippet.desription = "Angular UI Component";

            writer.writeLine(startTemplate);
            writer.nest(2);
            foreach (var field in defs.fieldDefs)
            {
                if (!field.showOnForm)
                {
                    continue;
                }
                if (field.controlType == ControlType.CheckBox)
                {
                    writer.writeMultiLine(codeHtmlControl(field, defs.entityNameLower, true));
                }
                else
                {
                    writer.writeLine(@$"<div class=""form-group"">");
                    writer.nest();
                    writer.writeLine(@$"<label for=""{field.fieldNameLower}"" class=""control-label"">{field.label}</label>");
                    writer.writeMultiLine(codeHtmlControl(field, defs.entityNameLower));
                    writer.unNest();
                    writer.writeLine("</div>");
                }
            }
            writer.nest(0);
            writer.writeLine(endTemplate);
            snippet.code = replaceNames(defs, writer.toString());

            return snippet;
        }

        public Snippet codeCss(EntityModel defs)
        {
            var template = @"";
            
            var writer = new CodeWriter();
            var snippet = new Snippet();
            snippet.header = "Form CSS";
            snippet.language = Language.CSS;
            snippet.desription = "Just as an example. Create your own as required";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }

    }
}
