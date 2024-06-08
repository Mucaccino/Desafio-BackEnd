export class ClientBase {
    /**
     * authorization token value
     */
    public static authToken: string | null = null

    constructor() {

    }

    setAuthToken(token: string) {
        ClientBase.authToken = token;
    }

    protected transformOptions(options: RequestInit): Promise<RequestInit> {
        debugger;
        if (ClientBase.authToken) {
            options.headers["Authorization"] = "bearer " + ClientBase.authToken 
        } else {
            console.warn("Authorization token have not been set please authorize first.");
        }
        return Promise.resolve(options);
    }

    protected transformResult(url: string, response: Response, processor: (response: Response) => any) {
        debugger;
        // TODO: Return own result or throw exception to change default processing behavior, 
        // or call processor function to run the default processing logic
        console.log("Service call: " + url);
        return processor(response);
    }
}