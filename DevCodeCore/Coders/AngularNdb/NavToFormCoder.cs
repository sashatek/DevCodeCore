using DevCodeCore.Models;
using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.AngularNdb
{
    class NavToFormCoder : BaseCoder
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
export class TripNavToFormComponent implements OnInit {
    @ViewChild(TripFormComponent) private tripFormComponent: TripFormComponent;

    trip: ModelWorker<TripModel> = new ModelWorker();
    closeResult: string;
    showForm = false;

    constructor(private tripService: TripService,
                private arptService: ArptLookupService,
                public refDataService: RefDataService, ) { }

    ngOnInit() {
      this.getTrips();
    }

    getTrips() {
      this.tripService.getAllTrips()
        .subscribe(trips => this.onGetTrips(trips));
    }

    onGetTrips(trips: TripModel[]) {
      this.trip.list = trips;
    }

    addTrip() {
      const model = this.newTrip();
      this.editTrip(model);
      this.showForm = true;
    }

    editTrip(model: TripModel) {
      // this.save(model);
      this.trip.model = model;
      this.showForm = true;
    }

    saveTrip(model: TripModel) {
      if (model.isNew) {
        this.tripService.addTrip(model)
          .subscribe(_ => {
            // model.transTypeDesc = findRef(this.refService.ref.transTypes, model.transTypeId).text;
            this.trip.list.unshift(model);
          });
      } else {
        this.tripService.saveTrip(model)
          .subscribe(_ => { });
      }

      this.showForm = false;
      this.trip.updateModel(model);
    }

    cancel(model: TripModel) {
      this.showForm = false;

    }

    deleteTrip(model: TripModel) {
      if (!confirm('Are you sure you want to delete?')) {
        return;
      }

      this.tripService.deleteTrip(model)
        .subscribe(_ => { });
      this.trip.list.splice(this.trip.list.indexOf(model), 1);
      this.trip.model = null;
      this.showForm = false;

    }

    newTrip() {
      const model = new TripModel();
      return model;
    }

}

";
            var snippet = new Snippet();
            snippet.header = "Navigate To Form Controller";
            snippet.language = Language.TypeScript;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }
        public Snippet codeHtml(EntityModel defs)
        {
            var template = @"
<div *ngIf=""!showForm"">
    <div>
        <button type=""button"" class=""btn btn-primary"" (click)=""addTrip()"">
            Add New Trip
        </button>
    </div>
  <div *ngFor=""let model of trip.list; let i = index"" class=""tripList"">
    <h4>
      <b>{{model.tripDate | date : ""MM/dd/yyyy""}} </b>
      <b>{{model.airportInfo?.text}}</b>
    </h4>
    {{model.airportInfo?.text2}}
    <p>{{model.transTypeDesc}}</p>
    <p>{{model.groupName}} {{model.groupSize}}</p>
    <button type=""button"" class=""btn btn-link"" (click)=""editTrip(model)"">Edit</button>
    <button type=""button"" class=""btn btn-link"" (click)=""deleteTrip(model)"">Delete</button>

  </div>
</div>

<div class=""col""  *ngIf=""showForm && trip.model"">
    <div style=""max-width:300px"" *ngIf=""trip.model"">
        <app-trip-form
        [model]=""trip.model""
        [enableDelete]=""false""
        (save)=""saveTrip($event)""
        (cancel)=""cancel($event)""
        (selete)=""deleteTrip($event)""
        ></app-trip-form>
    </div>
  </div>

";
            var snippet = new Snippet();
            snippet.header = "Navigate To Form";
            snippet.language = Language.HTML;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }

        public Snippet codeCss(EntityModel defs)
        {
            var template = @"
.tripList {
    position: relative;
    width: 220px;
    min-height: 220px;
    display: inline-block;
    margin: 1em;
    padding-top: 8px;
    text-align: center;
    -moz-box-shadow: 10px 10px 9px rgba(0, 0, 0, 0.22);
    -webkit-box-shadow: 10px 10px 9px rgba(0, 0, 0, 0.22);
    box-shadow: 10px 10px 9px rgba(0, 0, 0, 0.22);
    background-color: #ffc;
    vertical-align: top;
}
"; var writer = new CodeWriter();
            var snippet = new Snippet();
            snippet.header = "Navigate To Form  CSS";
            snippet.language = Language.CSS;
            snippet.desription = "Just as an example. Create your own as required";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }
    }
}
