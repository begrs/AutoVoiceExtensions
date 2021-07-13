import * as HTTP from 'http';
import fetch, { Response } from 'node-fetch';

export class WebEndpoint {

    constructor(private baseurl: string){

    }

    private genericError = "Local AVE web api server could not be reached.";

    public async startListening() {
        const url = this.baseurl + "listening/start";
        fetch(url).then(res => {
            if(!this.guard(res)){return;}
        });
    }

    public stopListening() {
        const url = this.baseurl + "listening/stop";
        fetch(url).then(res => {
            if(!this.guard(res)){return;}
        });
    }

    private guard(res: Response) : boolean{
        if (!res.ok) {
            console.debug("call to " + res.url + " returned code " + res.status + " " + res.statusText);
            console.error(this.genericError);
        }
        return res.ok;
    }

    public generateCommands(commands: { label: string, id: string }) {

    }
}