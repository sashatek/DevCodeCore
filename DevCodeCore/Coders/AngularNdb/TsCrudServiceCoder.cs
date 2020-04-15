using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.WebClient
{
    class TsCrudServiceCoder : BaseCoder
    {
        public Snippet codeService(EntityModel defs)
        {
            var template = @"
@Injectable({
  providedIn: 'root'
})
export class TripService {

  constructor(private http: HttpClient, private globals: Globals) { }

  getAllTrips() {
    const qry = '1';
    const url = `${this.globals.baseApiUrl}Trip/GetAll`;
    return this.http
    .get<TripModel[]>(url)
    .pipe(
      tap(response => this.onGetTrips(response)),
      catchError(e => this.handleError(e))
      );
  }


  onGetTrips(trips: TripModel[]) {
    for (const trip of trips) {
      // trip.tripDate_ = new Date(trip.tripDate);
      // trip.tripDate_ = new Date(Date.UTC(trip.tripDate_.getUTCFullYear(), trip.tripDate_.getUTCMonth(), trip.tripDate_.getUTCDate()));
      // trip.tripDate_ = isoToNgbDate(trip.tripDate);
      // trip.transTypeDesc = findRef()
      trip.isNew = false;
   }
    return trips;
  }

  addTrip(model: TripModel) {
    const url = `${this.globals.baseApiUrl}Trip/PostTrip`;
    return this.http
      .post<TripModel>(url, model)
       .pipe(
        tap(trip => {
          model.tripId = trip.tripId;
          model.isNew = false;
        }), // this.onGetTrips(response),
        catchError(this.handleError)
      );
  }

  saveTrip(model: TripModel) {
    const url = `${this.globals.baseApiUrl}Trip/${model.tripId}`;
    return this.http
      .put<TripModel>(url, model)
      .pipe(
        tap(trip => {
         }), // this.onGetTrips(response),
        catchError(this.handleError)
      );
  }
  deleteTrip(model: TripModel) {
    const url = `${this.globals.baseApiUrl}Trip/${model.tripId}`;
    return this.http
      .delete<TripModel>(url)
      .pipe(
        catchError(this.handleError)
      );
  }

  // handleError(s:string,model: TripModel){
  //     alert(""Error "" + s);
  // }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      let message = error.status + ' ' + error.statusText;
      if (error.error && error.error.message) {
        message += '\r' + error.error.message;
      }

      if (error.message) {
        message += '\r' + error.message;
      }
      // console.error(
      //   `Backend returned code ${error.status}, ` +
      //   `body was: ${error.error}`);
      alert(message);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  }
}
";

            var snippet = new Snippet();
            snippet.header = "HTTP CRUD Service";
            snippet.language = Language.TypeScript;
            snippet.desription = "";
            snippet.code = replaceNames(defs, template);

            return snippet;
        }

    }
}
