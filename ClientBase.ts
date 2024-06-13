export class ClientBase {
    /**
     * authorization token value
     */
    public static authToken: string | null = null

    constructor() {

    }

    setAuthToken(token: string | null) {
        ClientBase.authToken = token;
    }

    protected transformOptions(options: RequestInit): Promise<RequestInit> {
        if (ClientBase.authToken && options.headers) {
            const headers = options.headers as Record<string, string>;
            headers["Authorization"] = "bearer " + ClientBase.authToken
            options.headers = headers
        } else {
            console.warn("Authorization token have not been set please authorize first.");
        }
        return Promise.resolve(options);
    }

    protected transformResult(url: string, response: Response, processor: (response: Response) => any) {
        // TODO: Return own result or throw exception to change default processing behavior, 
        // or call processor function to run the default processing logic
        console.log("Service call: " + url);
        return processor(response);
    }
}