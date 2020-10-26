using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.AngularNdb
{
    class LookupControlCoder : BaseCoder
    {
        public Snippet codeController(EntityModel defs)
        {
            var template = @"
@Component({
  selector: 'app-airport-lookup',
  templateUrl: './airport-lookup.component.html',
  styleUrls: ['./airport-lookup.component.css']
})

export class ArptLookupComponent implements OnInit {
  searching = false;
  searchFailed = false;
  @Input() parentForm: FormGroup;
  @Input() formFieldName: string;

  @Output() airportSelect = new EventEmitter<ILookupItem>();

  constructor(private arptService: ArptLookupService) { }

  ngOnInit() {
    // this.parentForm.addControl();
  }

  lookupArpt = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap(() => this.searching = true),
      switchMap(term =>
        this.arptService.lookup(term).pipe(
          tap(() => this.searchFailed = false),
          catchError(() => {
            this.searchFailed = true;
            return of([]);
          }))
      ),
      tap(() => this.searching = false)
    )

    // formatter = (result: ILookupItem) => result.text;
    formatter(result: ILookupItem): string {
      return result.text + '   ' + result.text2;
    }
    formatterr(result: ILookupItem): string {
      return result.text + ' ' + result.text2;
    }

    select(event: NgbTypeaheadSelectItemEvent) {
      this.airportSelect.emit(event.item as ILookupItem);
    }
}

";

            var snippet = new Snippet();
            snippet.header = "Lookup UI Control Controller";
            snippet.language = Language.TypeScript;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }

        public Snippet codeHtml(EntityModel defs)
        {
            var template = @"
<div [formGroup]=""parentForm"">
<input id=""typeahead-http"" type=""text"" class=""form-control iata-box"" [ngbTypeahead]=""lookupArpt""
  placeholder=""IATA"" [inputFormatter]=""formatter"" [resultFormatter]=""formatterr"" name=""iata"" (selectItem)=""select($event)""
  onfocus=""this.select();"" onmouseup=""return false;"" [formControlName]=""formFieldName""  required/>

  <span *ngIf=""searching"">searching...</span>
  <div class=""invalid-feedback"" *ngIf=""searchFailed"">Sorry, suggestions could not be loaded.</div>
</div>
";

            var snippet = new Snippet();
            snippet.header = "Lookup UI Control HTML";
            snippet.language = Language.HTML;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }
    }
}
