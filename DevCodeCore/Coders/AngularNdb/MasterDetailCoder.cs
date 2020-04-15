using DevCodeCore.Models;
using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.AngularNdb
{
    class MasterDetailCoder : BaseCoder
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
export class TripMasterDetailComponent implements OnInit {
  @ViewChild(TripFormComponent) private tripFormComponent: TripFormComponent;

  trip: ModelWorker<TripModel> = new ModelWorker();
  closeResult: string;

  constructor(private tripService: TripService,
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
  }

  editTrip(model: TripModel) {
    if (this.tripFormComponent && this.tripFormComponent.isDitry()) {
      return;
    }
    // this.save(model);
    this.trip.model = model;
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
    this.trip.updateModel(model);
  }

  cancel(model: TripModel) {
    // this.trip.model = this.trip.model;
    // this.tripFormComponent.setModel(model);
    this.trip.model = null;

  }

  deleteTrip(model: TripModel) {
    if (!confirm('Are you sure you want to delete?')) {
      return;
    }

    this.tripService.deleteTrip(model)
      .subscribe(_ => { });
    this.trip.list.splice(this.trip.list.indexOf(model), 1);
    this.trip.model = null;
  }

  newTrip() {
    const model = new TripModel();
    return model;
  }
}
";
            var snippet = new Snippet();
            snippet.header = "Master-Detail Controller";
            snippet.language = Language.TypeScript;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }
        public Snippet codeHtml(EntityModel defs)
        {
            var template = @"
<div>
    <button type=""button"" class=""btn btn-primary"" (click)=""addTrip()"">
      Add New Trip
    </button>
  </div>
  <div class=""row"">
    <div class=""col"">
      <div class=""master-detail-list"">
        <div class=""list-group"">
          <a href=""#"" class=""list-group-item list-group-item-action d-flex justify-content-between align-items-center""
            *ngFor=""let model of trip.list; let i = index"" (click)=""editTrip(model)""
            [ngClass]=""{'active': this.trip.model == model}"">
            <div>
              <h5>{{model.tripDate | date : ""MM/dd/yyyy""}}, {{model.airportInfo.text}} - {{model.airportInfo.text2}}</h5>
              <p>{{model.transTypeDesc}}, {{model.groupName}}</p>
            </div>
            <span class=""badge badge-primary badge-pill"">{{model.groupSize}}</span>
            <!-- <h4>
          <b>{{model.tripDate | date : ""MM/dd/yyyy""}} </b>
          <b>{{model.airport?.text}}</b>
        </h4>
        {{model.airport?.text2}}
        <p>{{model.transTypeDesc}}</p>
        <p>{{model.groupName}} {{model.groupSize}}</p> -->
            <!-- <button type=""button"" class=""btn btn-outline-primary btn-sm"" (click)=""edit(model)"">Edit</button>
      <button type=""button"" class=""btn btn-outline-warning btn-sm"" (click)=""delete(model)"">Delete</button> -->
          </a>
        </div>
      </div>
    </div>
    <div class=""col"">
      <div style=""max-width:300px"" *ngIf=""trip.model"">
          <app-trip-form
          [model]=""trip.model""
          [enableDelete]=""true""
          (save)=""saveTrip($event)""
          (cancel)=""cancel($event)""
          (delete)=""deleteTrip($event)""
          ></app-trip-form>
      </div>
    </div>
  </div>
";
            var snippet = new Snippet();
            snippet.header = "Master-Detail";
            snippet.language = Language.TypeScript;
            snippet.desription = "Angular UI Component";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }

        public Snippet codeCss(EntityModel defs)
        {
            var template = @"

";
            var writer = new CodeWriter();
            var snippet = new Snippet();
            snippet.header = "Master-Detail";
            snippet.language = Language.CSS;
            snippet.desription = "Just as an example. Create your own as required";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }
    }
}
