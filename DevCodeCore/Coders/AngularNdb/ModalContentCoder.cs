using DevCodeCore.Models;
using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.AngularNdb
{
    class ModalContentCoder : BaseCoder
    {
        public Snippet[] code(EntityModel defs)
        {
            List<Snippet> snippets = new List<Snippet>();
            snippets.Add(codeController(defs));
            snippets.Add(codeHtml(defs));
            snippets.Add(codeCss(defs));
            return snippets.ToArray();
        }
        public Snippet codeController(EntityModel defs)
        {
            var template = @"
@Component({
  selector: 'app-trip-modal-form',
  templateUrl: './trip-modal-form.component.html',
  styleUrls: ['./trip-modal-form.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class TripModalFormComponent implements OnInit {
  @ViewChild(TripFormComponent) private tripFormComponent: TripFormComponent;

  model: TripModel = null;
  searching = false;
  searchFailed = false;

  constructor(private tripService: TripService,
              private arptService: ArptLookupService,
              public refDataService: RefDataService,
              private activeModal: NgbActiveModal) { }

  ngOnInit() {
  }

  saveTrip(model: TripModel) {
    this.activeModal.close(model);
  }

  saveClick() {
    this.tripFormComponent.saveTrip();
  }
  cancel() {
    this.activeModal.dismiss('Cancel');
  }

  isDirty() {
    return this.tripFormComponent?.tripForm.dirty;
  }
  isValid() {
    return this.tripFormComponent.tripForm.valid;
  }

}
";
            var snippet = new Snippet();
            snippet.header = "Modal Content Form Controller";
            snippet.language = Language.TypeScript;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }
        public Snippet codeHtml(EntityModel defs)
        {
            var template = @"
<div class=""modal-header"">
  <h4 class=""modal-title"">Hi there!</h4>
  <button type=""button"" class=""close"" aria-label=""Close"" (click)=""cancel()"">
    <span aria-hidden=""true"">&times;</span>
  </button>
</div>
<div class=""modal-body"">
  <app-trip-form [model]=""model"" [enableDelete]=""false"" [showButtons]=""false"" (save)=""saveTrip($event)""
    (cancel)=""cancel()""></app-trip-form>
</div>
<div class=""modal-footer"">
  <button type=""button"" class=""btn btn-primary"" (click)=""saveClick()"" [disabled]=""!(isDirty() && isValid())"">Save
    changes</button>
  <button type=""button"" class=""btn btn-warning"" (click)=""cancel()"">Cancel</button>
</div>
";
            var snippet = new Snippet();
            snippet.header = "Modal Content Form View";
            snippet.language = Language.HTML;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }

        public Snippet codeCss(EntityModel defs)
        {
            var template = @"
.modal-dialog {
    max-width: 20rem;
  }
"; var writer = new CodeWriter();
            var snippet = new Snippet();
            snippet.header = "Modal Content Form CSS";
            snippet.language = Language.CSS;
            snippet.desription = "Just as an example. Create your own as required";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }
    }
}
