import mysql.connector

# connect to DB
nextdb = mysql.connector.connect(
  host="localhost",
  user="root",
  password="",
  database="nextdb" # created the database at http://localhost/phpmyadmin
)
# Hej med dig god dag?
cursor = nextdb.cursor()

cursor.execute("SHOW DATABASES")
# print databases
for x in cursor:
  print(x)

cursor.execute("SHOW TABLES")

print() 
print("Tables:")

for x in cursor:
  print(x) 

# STORED PROCEDURE FOR TABLE CREATION 
create_table_procedure = """
CREATE PROCEDURE IF NOT EXISTS create_table(
    IN table_name VARCHAR(255),
    IN columns_definitions TEXT  
)
BEGIN
    -- Limit characters in table names against sql injection

    IF table_name NOT LIKE '%[^a-zA-Z0-9_ ]%' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid table name';
    END IF;

    -- Pass the "create table" query to sql and ensure table doesn't exist 

    SET @sql = CONCAT('CREATE TABLE IF NOT EXISTS ', table_name, ' (', columns_definitions, ')'); 
    
    -- Prepare and execute the dynamic SQL statement

    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
END
"""
cursor.execute(create_table_procedure)
nextdb.commit()

# STORED PROCEDURE FOR INSERT INTO USERS
insert_user_procedure="""
CREATE PROCEDURE IF NOT EXISTS insertusers(
    IN p_employee_id INTEGER,
    IN p_username VARCHAR(255),
    IN p_name VARCHAR(255),
    IN p_lastname VARCHAR(255),
    IN p_email VARCHAR(255),
    IN p_password VARCHAR(255)
)
BEGIN
    -- Check if the user already exists by email
    IF NOT EXISTS (SELECT 1 FROM users WHERE email = p_email) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Email already exists, please choose another!';
        
    ELSE
        -- Insert data into the 'users' table if the email doesn't exist
        INSERT INTO users (employee_id, username, name, lastname, email, password)
        VALUES (p_employee_id, p_username, p_name, p_lastname, p_email, p_password);
    END IF;
END
"""
cursor.execute(insert_user_procedure)
nextdb.commit()

### INSERT INTO MACHINES/MATERIALS/PROCESSES/CUSTOMERS/CALC_HISTORY/COMBINATIONS PROCEDURES
#
#
#                               UNDER CONSTRUCTION
#
#

### TRIGGERS ON INSERT, UPDATE AND DELETE
#
#
#                               UNDER CONSTRUCTION 
#
#

# DEFINE PARAMETERS TO INSERT
employee_id = 1
username = "sinbey"
name = "Sinan"
lastname = "Beysimov"
email = "sinan@mail.com"
password = "123456"

# CALL THE PROCEDURE TO INSERT USER
try:
    # Call the stored procedure
    cursor.callproc("InsertUsers", [employee_id, username, name, lastname, email, password])

    # Fetch the result (message) from the stored procedure
    message = None
    for result in cursor.stored_results():
        message = result.fetchall()

    # Commit the changes to the database
    nextdb.commit()

    # Check if a message was returned and print the appropriate message
    if message:
        print(message[0][0])  # Print the returned message
    else:
        print("User inserted successfully!")
except mysql.connector.Error as err:
    # Catch SQL errors and print them
    print(f"Error: {err}")


#CALL CREATE TABLE PROCEDURE
        # pas the new table name to table_name
table_name = ""
        #columns = """column_name DATA_TYPE PRIMARY KEY AUTO_INCREMENT, column_name_2 DATA_TYPE NOT NULL"""
columns = """
    """
cursor.callproc("create_table", [table_name, columns])
nextdb.commit() 

# CHECHK IF THE TABLE EXISTS IN THE DB
cursor.execute("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = %s", (table_name,))
table_exists = cursor.fetchone()[0]

### Print success or error message
if table_exists:
    print(f"Table '{table_name}' has been created successfully!")
else:
    print(f"Error: Table '{table_name}' was NOT created!")


### Close the connection
cursor.close()
nextdb.close()