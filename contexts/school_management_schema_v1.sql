-- School Management Platform
-- PostgreSQL Schema v1.0

CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- COMMON

CREATE TABLE schools(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 name varchar(200) NOT NULL,
 created_at timestamptz DEFAULT now()
);

-- IDENTITY

CREATE TABLE roles(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 name varchar(100) NOT NULL UNIQUE
);

CREATE TABLE users(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 email varchar(300) NOT NULL,
 password_hash text NOT NULL,
 role_id uuid REFERENCES roles(id),
 created_at timestamptz DEFAULT now()
);

-- REFERENCE

CREATE TABLE academic_sessions(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 name varchar(100),
 start_date date,
 end_date date,
 is_active boolean DEFAULT false
);

CREATE TABLE class_groups(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 name varchar(150),
 admission_prefix varchar(20),
 sequence_no int
);

CREATE TABLE classes(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 class_group_id uuid REFERENCES class_groups(id),
 name varchar(100),
 sequence_no int
);

CREATE TABLE sections(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 class_id uuid REFERENCES classes(id),
 name varchar(50),
 capacity int
);

CREATE TABLE scholar_types(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 name varchar(100),
 is_default boolean
);

CREATE TABLE religions(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 name varchar(100)
);

CREATE TABLE castes(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 name varchar(100),
 is_reserved_category boolean
);

CREATE TABLE qualifications(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 name varchar(200)
);

CREATE TABLE occupations(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 name varchar(200)
);

CREATE TABLE designations(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 name varchar(200)
);

CREATE TABLE states(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 name varchar(100)
);

CREATE TABLE districts(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 state_id uuid REFERENCES states(id),
 name varchar(100)
);

CREATE TABLE cities(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 district_id uuid REFERENCES districts(id),
 name varchar(100),
 pin_code varchar(20),
 std_code varchar(20)
);

-- STUDENT

CREATE TABLE students(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 admission_number varchar(100) UNIQUE,
 scholar_type_id uuid REFERENCES scholar_types(id),
 first_name varchar(100),
 middle_name varchar(100),
 last_name varchar(100),
 gender varchar(30),
 dob date,
 religion_id uuid REFERENCES religions(id),
 caste_id uuid REFERENCES castes(id),
 phone varchar(30),
 email varchar(300),
 created_at timestamptz DEFAULT now()
);

CREATE TABLE parents(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 qualification_id uuid REFERENCES qualifications(id),
 occupation_id uuid REFERENCES occupations(id),
 designation_id uuid REFERENCES designations(id),
 name varchar(300),
 phone varchar(50),
 email varchar(300)
);

CREATE TABLE student_parents(
 student_id uuid REFERENCES students(id),
 parent_id uuid REFERENCES parents(id),
 relationship varchar(50),
 PRIMARY KEY(student_id,parent_id)
);

CREATE TABLE addresses(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 student_id uuid REFERENCES students(id),
 address_type varchar(50),
 line1 text,
 city_id uuid REFERENCES cities(id)
);

CREATE TABLE student_enrollments(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 student_id uuid REFERENCES students(id),
 academic_session_id uuid REFERENCES academic_sessions(id),
 class_id uuid REFERENCES classes(id),
 section_id uuid REFERENCES sections(id),
 roll_number varchar(50),
 status varchar(50)
);

CREATE UNIQUE INDEX ux_roll
ON student_enrollments(
academic_session_id,
class_id,
section_id,
roll_number
);

-- BEHAVIOUR

CREATE TABLE behaviour_templates(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 school_id uuid REFERENCES schools(id),
 name varchar(300)
);

CREATE TABLE behaviour_items(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 template_id uuid REFERENCES behaviour_templates(id),
 name varchar(300),
 display_order int
);

CREATE TABLE behaviour_sheets(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 student_id uuid REFERENCES students(id),
 parent_id uuid REFERENCES parents(id),
 month int,
 year int,
 status varchar(50)
);

CREATE TABLE behaviour_entries(
 id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 sheet_id uuid REFERENCES behaviour_sheets(id),
 day_no int,
 behaviour_item_id uuid REFERENCES behaviour_items(id),
 value text
);
