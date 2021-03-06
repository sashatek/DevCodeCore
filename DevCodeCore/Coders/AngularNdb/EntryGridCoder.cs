﻿using DevCodeCore.Models;
using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.AngularNdb
{
    class EntryGridCoder : BaseCoder
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
  selector: 'app-trip-entry-grid',
  templateUrl: './trip-entry-grid.component.html',
  styleUrls: ['./trip-entry-grid.component.css'],
  providers: [
    { provide: NgbDateAdapter, useClass: NgbUTCStringAdapter },
    { provide: NgbDateParserFormatter, useClass: NgbDateFRParserFormatter }
]

})
export class TripEntryGridComponent implements OnInit {
    searching = false;
    searchFailed = false;

    trip: ModelWorker<TripModel> = new ModelWorker();
    tripForm: FormGroup = null;
    tripForms: FormArray;

    constructor(private tripService: TripService,
                public refDataService: RefDataService,
                private fb: FormBuilder) {
        this.tripForm = fb.group({
            tripForms: fb.array([])
        });
        // this.tripForms = <FormArray>this.tripFormGroup.controls['tripForms'];
        this.tripForms = this.tripForm.controls.tripForms as FormArray;
    }

    ngOnInit() {
        this.getTrips();
    }

    getTrips() {
        this.tripService.getAllTrips()
            .subscribe(trips => this.onGetTrips(trips));
    }
    private initForm() {
        for (const model of this.trip.list) {
            this.tripForms.push(this.initFormItem(model));
        }
    }

    private initFormItem(model: TripModel) {
        return this.fb.group({
$$assign1$$});
    }

    onGetTrips(trips: TripModel[]) {
        this.trip.list = trips;
        this.initForm();
        this.addLine();
    }

    arptSelect(event: NgbTypeaheadSelectItemEvent) {
        // Place holder
    }

    // CRUD
    save(index: number, form: FormGroup) {
        const model = this.trip.model = this.trip.list[index];
        // const model: TripModel = { ...this.trip.model, ...form.value };
$$assign2$$
        form.markAsPristine();
        form.markAsUntouched();
        if (model.isNew) {
            this.addLine();
            this.tripService.addTrip(model)
                .subscribe(_ => { });
        } else {
            this.tripService.saveTrip(model)
                .subscribe(_ => { });
        }
        // this.trip.updateModel(model);
    }

    cancel(index: number) {
        this.tripForms.controls[index] = this.initFormItem(this.trip.list[index]);
    }

    delete(index: number) {
        const model = this.trip.list[index];
        if (!confirm('Are you sure you want to delete?')) {
            return;
        }
        this.tripService.deleteTrip(model)
            .subscribe(_ => { });
        index = this.trip.list.indexOf(model);
        this.trip.list.splice(index, 1);
        this.tripForms.removeAt(index);
    }

    private addLine() {
        const model = new TripModel();
        this.trip.list.push(model);
        this.tripForms.push(this.initFormItem(model));
        return model;
    }

}
";

            var snippet = new Snippet();
            snippet.header = "Entry Grid Controller";
            snippet.language = Language.TypeScript;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            var assign1 = makeFormGroup(defs,3);
            var assign2 = makeFormGetValue(defs,2);
            snippet.code = snippet.code
                 .Replace("$$assign1$$", assign1)
                 .Replace("$$assign2$$", assign2);
            return snippet;
        }

 
        public Snippet codeHtml(EntityModel defs)
        {
            string startTemplate = @"
<form [formGroup]=""tripForm"">
    <table class=""datatable datatableL"">
        <tr>
$$Headers$$
        </tr>
        <tr *ngFor=""let tripForm of tripForms.controls; let i = index"" [formGroup]=""tripForm"">";


            var endTemplate = @"    <td>
                <button type=""button"" class=""btn btn-primary btn-xs"" (click)=""save(i,tripForm)""
                    [disabled]=""!(tripForm.dirty && tripForm.valid)"">
                    Save
                </button>
            </td>
            <td>
                <button type=""button"" class=""btn btn-secondary btn-xs"" (click)=""cancel(i)""
                    [disabled]=""!tripForm.dirty"">
                    Cancel
                </button>
            </td>
            <td>
                <button type=""button"" class=""btn btn-warning  btn-xs"" (click)=""delete(i)""
                    *ngIf=""!trip.list[i].isNew"">
                    Delete
                </button>
            </td>
            <!--<td>
                <button type=""button"" class=""btn btn-outline-success btn-xs"" (click)=""save(i,tripForm)""
                    [disabled]=""!(tripForm.dirty && tripForm.valid)"">
                    <i class=""material-icons"" >save</i>
                </button>
            </td>
            <td>
                <button type=""button"" class=""btn btn-outline-danger btn-xs"" (click)=""cancel(i)""
                    [disabled]=""!tripForm.dirty"">
                    <i class=""material-icons"" >cancel</i>
                </button>
            </td>
            <td>
                <button type=""button"" class=""btn btn-outline-warning  btn-xs"" (click)=""delete(i, tripForm)""
                    *ngIf=""!trip.list[i].isNew"">
                    <i class=""material-icons"" >delete</i>
                </button>
            </td>-->

        </tr>
    </table>
</form>";


            //            var endtemplate = @"

            //";

            var writer = new CodeWriter();
            var snippet = new Snippet();
            snippet.header = "Entry Grid HTML View";
            snippet.language = Language.HTML;
            snippet.desription = "Angular UI Component";

            var w = new CodeWriter();
            w.nest(3);
            foreach (var field in defs.fieldDefs)
            {
                w.writeLine($"<th>{field.label}</th>");
            }

            writer.writeLine(startTemplate.Replace("$$Headers$$", w.toString()));
            writer.nest(3);
            foreach (var field in defs.fieldDefs)
            {
                if (field.showOnForm)
                {
                    writer.writeLine("<td>");
                    writer.nest();
                    writer.writeMultiLine(codeHtmlControl(field, defs.entityNameLower));
                    writer.unNest();
                    writer.writeLine("</td>");
                }
            }
            writer.unNest();
            writer.writeLine(endTemplate);
            snippet.code = replaceNames(defs, writer.toString());

            return snippet;
        }


        public Snippet codeCss(EntityModel defs)
        {
            var template = @"
.date-box {
	width: 110px;
}

.arpt-name{
    white-space:nowrap;
}

.date-col{
    white-space:nowrap;
	width: 170px;
}

.iata-box {
	width: 70px;
}
.group-size-box {
	width: 70px;
}
"; var writer = new CodeWriter();
            var snippet = new Snippet();
            snippet.header = "Entry Grid CSS";
            snippet.language = Language.CSS;
            snippet.desription = "Just as an example. Create your own as required";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }
        public Snippet codeTest(EntityModel defs)
        {
            var template = @"

";
            var snippet = new Snippet();
            snippet.header = "Entry Grid HTML View";
            snippet.language = Language.TypeScript;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }


    }

}


