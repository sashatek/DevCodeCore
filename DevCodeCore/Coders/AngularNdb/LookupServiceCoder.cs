using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.AngularNdb
{
    class LookupServiceCoder : BaseCoder
    {
        public Snippet codeService(EntityModel defs)
        {
            var template = @"
@Injectable({
    providedIn: 'root'
})
export class ArptLookupService {

    constructor(private http: HttpClient, private globals: Globals) { }

    lookup(term: string) {
        // const url = `${this.globals.baseAppUrl}/api/Lookup/Iata/${term}`
        const url = `${this.globals.baseApiUrl}Lookup/Iata/${term}`;
        if (term === '') {
            return of([]);
        }

        return this.http
            .get<ILookupItem[]>(url).pipe(
                map(response => response)
            );
    }
}
";

            var snippet = new Snippet();
            snippet.header = "HTTP Lookup Service";
            snippet.language = Language.TypeScript;
            snippet.desription = "";

            snippet.code = replaceNames(defs, template);

            return snippet;
        }
    }
}
