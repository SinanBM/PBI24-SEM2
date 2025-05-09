from flask import Flask, render_template, request

app = Flask(__name__)

# ROUTE: Startside / login-side
@app.route("/")
def login():
    return render_template("login.html")

# ROUTE: Vis anmodningsformularen
@app.route("/request_access")
def request_access():
    return render_template("request_access.html")

# ROUTE: Modtag og håndtér anmodningen
@app.route("/anmod", methods=["POST"])
def handle_request():
    # 1. Hent data fra formularen
    navn = request.form["fullname"]
    email = request.form["email"]

    # 2. Print det (senere gemmer vi det)
    print("Ny anmodning:")
    print("Navn:", navn)
    print("E-mail:", email)

    return "Tak! Din anmodning er modtaget. Du får svar snarest."
