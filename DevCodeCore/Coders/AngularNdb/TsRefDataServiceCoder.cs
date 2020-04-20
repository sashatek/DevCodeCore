using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.AngularNdb
{
    class TsRefDataServiceCoder : BaseCoder
    {
        public Snippet codeService(EntityModel defs)
        {
            var template = @"
@Injectable({
    providedIn: 'root'
})
export class RefDataService {

    refData: RefDataModel = null;

    constructor(private http: HttpClient, private globals: Globals) { }

    getRefs() {
        const url = `${this.globals.baseApiUrl}RefData/GetAll`;
        return this.http
            .get<RefDataModel>(url)
            .pipe(
                tap(response => this.onGetRefs(response)
                ));

    }

    onGetRefs = (ref: RefDataModel) => {
        this.refData = ref;
    }

    getRefDataById(refData: ILookupItem[], id: number){
        for (const ref of refData) {
            if (ref.id === id){
                return ref;
            }
        }
        return null;
    }
}
";

            var snippet = new Snippet();
            snippet.header = "HTTP Reference Data Service";
            snippet.language = Language.TypeScript;
            snippet.desription = "";

            snippet.code = replaceNames(defs, template);

            return snippet;
        }
    }
}
