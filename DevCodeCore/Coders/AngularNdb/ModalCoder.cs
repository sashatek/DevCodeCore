using DevCodeCore.Models;
using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.AngularNdb
{
    class ModalCoder : BaseCoder
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
export class TripModalComponent implements OnInit {

    trip: ModelWorker<TripModel> = new ModelWorker();

    closeResult: string;

    constructor(private tripService: TripService,
                private arptService: ArptLookupService,
                public refDataService: RefDataService,
                private modalService: NgbModal) { }
    @Input() model: TripModel;

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
      this.edit(model);
  }

    edit(model: TripModel) {
      this.trip.model = model;
      const modalRef = this.modalService.open(TripModalFormComponent);
      modalRef.componentInstance.model = model;
      modalRef.result.then((result) => {
        this.save(result);
      }, (reason) => {
        // this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      });
    }

    // CRUD
    private save(model: TripModel) {
      if (model.isNew) {
        this.tripService.addTrip(model)
          .subscribe(_ => {
            // model.transTypeDesc = findRef(this.Data.ref.transTypes, model.transTypeId).text;
            this.trip.list.unshift(model);
           });
      } else {
        this.tripService.saveTrip(model)
          .subscribe(_ => { });
      }
      this.trip.updateModel(model);
    }

    delete(model: TripModel) {
      if (!confirm('Are you sure you want to delete?')) {
        return;
      }

      this.tripService.deleteTrip(model)
        .subscribe(_ => { });
      this.trip.list.splice(this.trip.list.indexOf(model), 1);
    }

    newTrip() {
      const model = new TripModel();
      return model;
  }

    private getDismissReason(reason: any): string {
      if (reason === ModalDismissReasons.ESC) {
        return 'by pressing ESC';
      } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
        return 'by clicking on a backdrop';
      } else {
        return `with: ${reason}`;
      }
    }
}
";
            var snippet = new Snippet();
            snippet.header = "Modal Dialog Controller";
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
<div>
  <div *ngFor=""let model of trip.list; let i = index"" class=""tripList"">
    <!-- <p style=""position: absolute; bottom: 10px; right: 10px"">
      <a href=""#"" class=""btn btn-link"">Edit</a>
            <a href=""#"" class=""btn btn-link"">Delete</a>
      <button type=""button"" class=""btn btn-primary btn-xs"" ng-click=""gc.editTrip(model, tripForm)"">
        Edit
      </button>
      <button type=""button"" class=""btn btn-warning btn-xs"" ng-click=""gc.deleteTrip(model, tripForm)""
        ng-show=""!model.isNew"">
        Delete
      </button>
    </p> -->
    <h4>
      <b>{{model.tripDate | date : ""MM/dd/yyyy""}} </b>
      <b>{{model.airportInfo?.text}}</b>
    </h4>
    {{model.airportInfo?.text2}}
    <p>{{model.transTypeDesc}}</p>
    <p>{{model.groupName}} {{model.groupSize}}</p>
    <button type=""button"" class=""btn btn-outline-primary btn-sm"" (click)=""edit(model)"">Edit</button>
    <button type=""button"" class=""btn btn-outline-warning btn-sm"" (click)=""delete(model)"">Delete</button>

  </div>

  <!-- <div class=""card d-inline-block text-center border-primary"" style=""width: 18rem;"" *ngFor=""let model of trip.list; let i = index"">
    <div class=""card-body"">
      <h3>
        <b>{{model.tripDate | date : ""MM/dd/yyyy""}} </b>
        <b>{{model.airport?.text}}</b>
    </h3>
        {{model.airport?.text2}}
        <p>{{model.transTypeDesc}}</p>
        <p>{{model.groupName}} {{model.groupSize}}</p>
      <a href=""#"" class=""card-link"">Edit</a>
      <a href=""#"" class=""card-link"">Delete</a>
    </div>
  </div> -->
</div>
";
            var snippet = new Snippet();
            snippet.header = "Modal";
            snippet.language = Language.TypeScript;
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
            snippet.header = "Modal";
            snippet.language = Language.CSS;
            snippet.desription = "Just as an example. Create your own as required";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }
    }
}
