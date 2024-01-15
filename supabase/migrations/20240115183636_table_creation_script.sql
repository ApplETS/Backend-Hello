create table
  moderator (
    id bigint primary key generated always as identity,
    name text not null,
    email text not null
  );

create table
  organizer (
    id bigint primary key generated always as identity,
    name text not null,
    email text not null,
    organisation text not null,
    activity_area text not null
  );

create table
  publication (
    id bigint primary key generated always as identity,
    title text not null,
    content text not null,
    image_url text,
    state text not null,
    moderator_id bigint,
    organizer_id bigint not null,
    publication_date timestamp with time zone not null,
    foreign key (moderator_id) references moderator (id),
    foreign key (organizer_id) references organizer (id)
  );

create table
  event (
    id bigint primary key generated always as identity,
    title text not null,
    content text not null,
    image_url text,
    state text not null,
    moderator_id bigint,
    organizer_id bigint not null,
    publication_date timestamp with time zone not null,
    event_date timestamp with time zone not null,
    foreign key (moderator_id) references moderator (id),
    foreign key (organizer_id) references organizer (id)
  );

create table
  report (
    id bigint primary key generated always as identity,
    reason text not null,
    date timestamp with time zone default timezone ('utc'::text, now()) not null
  );

create table
  tag (
    id bigint primary key generated always as identity,
    name text not null,
    priority_value int not null,
    parent_tag_id bigint,
    foreign key (parent_tag_id) references tag (id)
  );

